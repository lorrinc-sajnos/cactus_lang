using System;
using Antlr4.Runtime;
using CactusLang.Compiling;
using CactusLang.Compiling.Codefiles;
using CactusLang.Model;
using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Entities.Structs;
using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.CodeStructure.FileContent;
using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Model.CodeStructure.Statements.ControlFlow;
using CactusLang.Model.Errors;
using CactusLang.Model.Scopes;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Model.Types.Abstract;
using CactusLang.Model.Types.PrimitiveTypes;
using CactusLang.Semantics.Errors;
using CactusLang.Util;
using GP = GrammarParser;

namespace CactusLang.Semantics;

public class SemanticFileAnalyzer : GrammarBaseVisitor<SemanticFileAnalyzer.StatusCode> {
    public enum StatusCode {
        SUCCESS,
        WARN,
        ERR
    }

    public const string CCTS_STD_FOLDER = "../../../../ccts_lang/cactus_std/std/";

    private readonly SemanticAnalyzer _analyzer;

    private readonly GrammarParser _parser;
    public FileTypeTable FileTypeTable { get; }

    private readonly FileScope _fileScope;
    private readonly TagFactory _tagFactory;
    public CactusFile CurrentFile { get; }

    public ErrorHandler ErrorHandler { get; }

    private Scope CurrentScope => _fileScope.CurrentScope;

    private CodeBlock _currentBlock;


    public SemanticFileAnalyzer(SemanticAnalyzer analyzer, CactusFile file) {
        _analyzer = analyzer;
        CurrentFile = file;

        //Antlr
        var srcFile = file.SourceFile;
        srcFile.ReadAllCode();

        var inputStream = new AntlrInputStream(srcFile.CodeText!);
        var lexer = new GrammarLexer(inputStream);
        lexer.RemoveErrorListeners();
        var tokenStream = new CommonTokenStream(lexer);
        _parser = new GrammarParser(tokenStream);

        ErrorHandler = CurrentFile.ErrorHandler;
        //Remove error listeners so it doesn't print it to the log
        _parser.RemoveErrorListeners();
        _parser.AddErrorListener(ErrorHandler.ErrorListener);

        _fileScope = new FileScope(CurrentFile, ErrorHandler);
        FileTypeTable = new FileTypeTable(file);
        _tagFactory = new();
    }

    public void AnalyzeFile() {
        GP.CodefileContext ctx = _parser.codefile();

        Debug.LogLine($"Start index:\t{ctx.Start.StartIndex}");
        Debug.LogLine($"Stop index:\t{ctx.Stop.StopIndex}");

        VisitHeaderStatemenets(ctx);
        RegisterStructs(ctx);

        //After registering structs, the Type system is completed, can begin registering functions
        RegisterFileScope(ctx);

        Debug.LogLine("Registration finished.");
        Debug.LogLine("Beginning analyzation.");
        AnalyzeCodeFile(ctx);
    }

    #region Registering

    private void VisitHeaderStatemenets(GP.CodefileContext ctx) {
        foreach (var headerStatement in ctx.headerStatement()) {
            VisitHeaderStatement(headerStatement);
        }
    }

    public override StatusCode VisitIncludeFile(GP.IncludeFileContext ctx) {
        CactusGrove grove = _analyzer.Grove;

        FileID fileId;
        //Including STD
        if (ctx.filepath().groveId() != null) {
            string groveName = ctx.filepath().groveId().GetText();
            if (groveName != "std") {
                ErrorHandler.Error(CactusError.GROVE_DOENST_EXISTS, ctx, groveName);
                return StatusCode.ERR;
            }

            var path = ctx.filepath().GetText();
            var difRoot = CCTS_STD_FOLDER;
            fileId = FileID.FromRelPathDcl(path, grove, difRoot);
        }
        else {
            var path = ctx.filepath().GetText();
            fileId = FileID.FromRelPathDcl(path, grove);
        }

        string truePath = fileId.AsRelPathWExtention();
        string alias = ctx.fileAlias().GetText();


        //Add the file
        try {
            CactusFile? includedFile = _analyzer.RequestFile(fileId);

            if (includedFile is null) {
                ErrorHandler.Error(CactusError.FILE_DOENST_EXISTS, ctx, truePath);
                return StatusCode.ERR;
            }

            CurrentFile.IncludeManager.AddIncludedFile(alias, includedFile);
        }
        catch (Exception e) {
            ErrorHandler.Error(CactusError.FILE_ERROR, ctx, truePath);
            return StatusCode.ERR;
        }

        return StatusCode.SUCCESS;
    }


    private void RegisterStructs(GP.CodefileContext ctx) {
        var statements = ctx.fileStatement();
        //First create the structs
        foreach (var statement in statements) {
            if (statement.structDcl() != null) {
                var structDcl = statement.structDcl();
                string structName = structDcl.structName().GetText();
                CactusStruct cactusStruct = new CactusStruct(structName, CurrentFile);
                Debug.LogLine($"Struct {structName} declared");

                FileTypeTable.DeclareStruct(cactusStruct);
                bool success = CurrentFile.DeclStructStore.AddStruct(cactusStruct); //TODO err Already defined?

                if (!success) {
                    ErrorHandler.Error(CactusError.ALREADY_DEFINED, structDcl.structName());
                    return;
                }
            }
        }

        //Then go over the header in their bodies, so they can cross-reference
        foreach (var statement in statements) {
            if (statement.structDcl() != null) {
                var structDcl = statement.structDcl();
                CactusStruct? cactusStruct = FileTypeTable.GetStructFromTokens(structDcl);
                Debug.LogLine($"@Struct  {cactusStruct.Name}:");

                var structBody = structDcl.structBody();

                //Instance Methods
                foreach (var funcDec in structBody.funcDcl()) {
                    RegisterStructFuncDcl(cactusStruct, funcDec);
                    Debug.LogLine($"\tAdded function: {funcDec.funcDclHeader().GetText()}");
                }

                //Fields
                foreach (var fieldDcl in structBody.fieldDcl()) {
                    RegisterStructFieldDcl(cactusStruct, fieldDcl);
                    Debug.LogLine($"\tAdded variable: {fieldDcl.GetText()}");
                }
            }
        }
    }

    private void RegisterFileScope(GP.CodefileContext ctx) {
        var globStatement = ctx.fileStatement();

        foreach (var fileStatement in globStatement) {
            if (fileStatement.funcDcl() != null) {
                var funcDcl = fileStatement.funcDcl();
                RegisterFileFuncDcl(funcDcl);
            }
            else if (fileStatement.fileVarDcl() != null) {
                var globVarCtx = fileStatement.fileVarDcl();
                RegisterFileVarDcl(globVarCtx);
            }
            else if (fileStatement.tags() != null) {
                CurrentFile.AddTags(_tagFactory.CreateTags(fileStatement.tags()));
            }
        }
    }

    private void RegisterFileFuncDcl(GP.FuncDclContext ctx) {
        var header = ctx.funcDclHeader();
        string funcName = header.funcName().GetText();
        CactusType returnType = FileTypeTable.GetFromCtx(header.returnType().type());
        FunctionSymbol funcSymb = new FunctionSymbol(returnType, funcName, CurrentFile);

        foreach (var param in header.param()) {
            string pName = param.paramName().GetText();
            CactusType pType = FileTypeTable.GetFromCtx(param.type());
            funcSymb.AddParameter(new VariableSymbol(pType, pName));
        }

        FileFunction fileFunc = new FileFunction(funcSymb, CurrentFile);
        var result = CurrentFile.Functions.AddFunction(fileFunc);
        if (result == false) {
            ErrorHandler.ErrorInExpression(CactusError.OVERLOAD_DOESNT_MATCH_RET_TYPE, ctx, header.funcName());
        }

        if (header.tags() != null)
            fileFunc.TagContainer.AddTags(_tagFactory.CreateTags(header.tags()));

        Debug.LogLine($"Func {funcSymb.Name} declared.");
    }

    private void RegisterFileVarDcl(GP.FileVarDclContext ctx) {
        var varCtx = ctx.varDcl();

        var varSymbols = SemanticUtil.GetVarSymbols(varCtx, FileTypeTable);

        foreach (VariableSymbol symb in varSymbols) {
            FileVariable variable = new FileVariable(symb, CurrentFile);

            var result = _fileScope.AddVariable(symb);
            CurrentFile.Fields.AddField(variable);

            if (!result)
                ErrorHandler.Error(CactusError.ALREADY_DEFINED, ctx);

            Debug.LogLine($"GlobVar Added: {symb.Type.Name}   {symb.Name}");
        }
    }


    private void RegisterStructFuncDcl(CactusStruct cactusStruct, GP.FuncDclContext ctx) {
        var header = ctx.funcDclHeader();
        string funcName = header.funcName().GetText();
        CactusType returnType = FileTypeTable.GetFromCtx(header.returnType().type());
        FunctionSymbol funcSymb = new FunctionSymbol(returnType, funcName, CurrentFile);

        foreach (var param in header.param()) {
            string pName = param.paramName().GetText();
            CactusType pType = FileTypeTable.GetFromCtx(param.type());
            funcSymb.AddParameter(new VariableSymbol(pType, pName));
        }

        StructMethod structFunc = new StructMethod(CurrentFile, funcSymb, cactusStruct);
        cactusStruct.Methods.AddFunction(structFunc);
    }

    private void RegisterStructFieldDcl(CactusStruct cactusStruct, GP.FieldDclContext ctx) {
        List<VariableSymbol> varSymbols = new List<VariableSymbol>();
        CactusType type = FileTypeTable.GetFromCtx(ctx.type());
        foreach (var varName in ctx.varName()) {
            varSymbols.Add(new VariableSymbol(type, varName.GetText()));
        }

        foreach (VariableSymbol symb in varSymbols) {
            cactusStruct.Fields.AddField(new StructField(symb, cactusStruct));
            Debug.LogLine($"\tAdded: {symb.Type.Name}   {symb.Name}");
        }
    }

    #endregion

    private void AnalyzeCodeFile(GP.CodefileContext ctx) {
        foreach (var statement in ctx.fileStatement()) {
            //IncludedFile function bodies
            if (statement.funcDcl() != null) {
                var funcDcl = statement.funcDcl();

                ProcessFunctionBody(funcDcl);
            }
            //Structs function bodies
            else if (statement.structDcl() != null) {
                var structDcl = statement.structDcl();
                CactusStruct structType = FileTypeTable.GetStructFromTokens(structDcl);
                Debug.LogLine($"\n---@Struct {structType.Name} ENTERED");

                //Loop through the functions of the struct
                var structFuncs = structDcl.structBody().funcDcl();
                _fileScope.StepInStructFunc(structType);

                foreach (var funcDclCtx in structFuncs) {
                    ProcessFunctionBody(funcDclCtx);
                }

                _fileScope.StepOut();
                Debug.LogLine($"\n---@Struct {structType.Name} EXITED");
                Debug.LogLine();
            }
        }
    }

    private void ProcessFunctionBody(GP.FuncDclContext ctx) {
        CactusFunction? func =
            CurrentScope.GetMatchingFunction(
                SemanticUtil.GetSignatureFromDclCtx(ctx.funcDclHeader(), FileTypeTable)
            );

        //Should be always non-null
        if (func is null) return;

        _currentBlock = func.CodeBody;

        Debug.LogLine($"FUNC {ctx.funcDclHeader().GetText()} ENTERED");
        //_fileScope.StepIn();
        //Add the parameters to the current scope
        foreach (var param in func!.Symbol.GetParameters()) {
            CurrentScope.AddVariable(param); //In theory, scope is empty at this point, thus an error cannot occur.
        }

        //If function has C code, trust that it will work
        if (ctx.ppc__C_Code_Body() != null) {
            _currentBlock.AddStatement(new RawCCodeStatement(_currentBlock, ctx.ppc__C_Code_Body().GetText(), true));
            return;
        }

        if (ctx.ppc__C_Func_Map() != null) {
            _currentBlock.AddStatement(new RawCCodeStatement(_currentBlock,
                ctx.ppc__C_Func_Map().ppc__C_Func().GetText(), false));
            return;
        }

        if (ctx.funcLamdBody() != null) {
            var exprCtx = ctx.funcLamdBody().expression();
            var lamdbaExpr = EvaluateExpression(exprCtx);

            if (func.ReturnType.Equals(NumberType.VOID)) {
                _currentBlock.AddStatement(new ExpressionStatement(_currentBlock, lamdbaExpr));
            }
            else {
                _currentBlock.AddStatement(new ReturnStatement(_currentBlock, lamdbaExpr));
            }

            return;
        }

        //Handle the most common case:
        var codeBody = ctx.codeBody();

        VisitCodeBody(codeBody);

        //_fileScope.StepOut();


        Debug.LogLine($"FUNC {ctx.funcDclHeader().GetText()} EXITED\n");
    }

    private void FillVarDclStatement(GP.VarDclContext ctx, VarDclStatement varDclStatement) {
        CactusType dclType = FileTypeTable.GetFromCtx(ctx.type());

        foreach (var dclBody in ctx.varDclBody()) {
            var varSymbol = new VariableSymbol(dclType, dclBody.varName().GetText());

            //Typecheck:
            if (dclBody.expression() != null) {
                GP.ExpressionContext asgExprCtx = dclBody.expression();
                var asgExpression = EvaluateExpression(
                    asgExprCtx,
                    whenErrExpr: DEFAULT,
                    otherErrHandling: result => {
                        if (!result.GetResultType().CanBeUsedAs(dclType)) {
                            ErrorHandler.ErrorInExpression(CactusError.TYPE_MISMATCH, ctx, result.GetResultType(),
                                dclType);
                            //continue;
                        }
                    });


                varDclStatement.AddBody(new VarDclStatement.Body(varSymbol, asgExpression));
            }
            else {
                varDclStatement.AddBody(new VarDclStatement.Body(varSymbol));
            }

            _fileScope.CurrentScope.AddVariable(varSymbol);
        }
    }

    public override StatusCode VisitCodeBody(GrammarParser.CodeBodyContext context) {
        _fileScope.StepIn();
        Debug.LogLine("Entering code body.");
        var result = base.VisitCodeBody(context);
        _fileScope.StepOut();
        Debug.LogLine("Exiting code body.");
        return result;
    }

    #region Statement Handling

    private ExpressionFactory CreateFactory(GP.ExpressionContext node) =>
        new( CurrentFile, this, _fileScope.CurrentScope, node);


    public override StatusCode VisitStatement(GP.StatementContext ctx) {
        //Debug.LogLine("statement:"+ctx.GetText());
        return base.VisitStatement(ctx);
    }

    public override StatusCode VisitVarDcl(GrammarParser.VarDclContext ctx) {
        VarDclStatement varDclStatement = new(_currentBlock);

        FillVarDclStatement(ctx, varDclStatement);

        _currentBlock.AddStatement(varDclStatement);
        return base.VisitVarDcl(ctx);
    }

    public override StatusCode VisitExpressionStatement(GrammarParser.ExpressionStatementContext ctx) {
        var exprCtx = ctx.expression();
        var expression = EvaluateExpression(exprCtx);


        var res = base.VisitExpressionStatement(ctx);

        _currentBlock.AddStatement(new ExpressionStatement(_currentBlock, expression));
        return res;
    }

    public override StatusCode VisitFreeStatement(GrammarParser.FreeStatementContext ctx) {
        var exprCtx = ctx.expression();
        var expression = EvaluateExpression(exprCtx,
            whenErrExpr: DEFAULT,
            otherErrHandling: result => {
                if (result.GetResultType() is not PointerType) {
                    ErrorHandler.ErrorInExpression(CactusError.FREE_ERROR, ctx, ctx.expression().GetText());
                }
            }
        );

        _currentBlock.AddStatement(new FreeStatement(_currentBlock, expression));
        return base.VisitFreeStatement(ctx);
    }


    public override StatusCode VisitReturnStatement(GrammarParser.ReturnStatementContext ctx) {
        var exprCtx = ctx.expression();
        var expression = EvaluateExpression(exprCtx);


        _currentBlock.AddStatement(new ReturnStatement(_currentBlock, expression));

        return base.VisitReturnStatement(ctx);
    }

    #region Control

    public override StatusCode VisitIfStatement(GP.IfStatementContext ctx) {
        var ifCondExprCtx = ctx.ifCond().expression();
        var ifCond = EvaluateExpression(
            ifCondExprCtx,
            whenErrExpr: DEFAULT,
            otherErrHandling: result => {
                if (result.GetResultType() != NumberType.BOOL) {
                    ErrorHandler.ErrorInExpression(CactusError.CONDITION_ERROR, ctx.ifCond(), result.GetResultType());
                }
            });

        IfStatement ifStatement = new(ifCond, _currentBlock);

        //Visit inner block
        var storeCurrentBlock = _currentBlock;
        _currentBlock = ifStatement.Body;
        var result = base.VisitCodeBody(ctx.codeBody());

        _currentBlock = storeCurrentBlock;
        //Visit else if there is one
        if (ctx.elseStatement() != null)
            ifStatement.Else = EvaluateElseBranch(ifStatement, ctx.elseStatement());

        _currentBlock.AddStatement(ifStatement);
        return result;
    }

    private ElseBranch EvaluateElseBranch(IElseBranchOwner parent, GP.ElseStatementContext ctx) {
        Expression? elifCond = null;
        if (ctx.ifCond() != null) {
            elifCond = EvaluateExpression(
                ctx.ifCond().expression(),
                whenErrExpr: DEFAULT,
                otherErrHandling: result => {
                    if (result.GetResultType() != NumberType.BOOL) {
                        ErrorHandler.ErrorInExpression(CactusError.CONDITION_ERROR, ctx.ifCond(), result.GetResultType());
                    }
                });
        }

        ElseBranch elseBranch = new(parent, elifCond, _currentBlock);

        //Visit inner block
        var storeCurrentBlock = _currentBlock;

        _currentBlock = elseBranch.Body;
        base.VisitCodeBody(ctx.codeBody());
        _currentBlock = storeCurrentBlock;

        if (ctx.elseStatement() != null)
            elseBranch.Else = EvaluateElseBranch(elseBranch, ctx.elseStatement());

        return elseBranch;
    }

    public override StatusCode VisitWhileLoop(GP.WhileLoopContext ctx) {
        var loopCondExprCtx = ctx.loopCond().expression();
        var loopCond = EvaluateExpression(
            loopCondExprCtx,
            whenErrExpr: DEFAULT,
            otherErrHandling: result => {
                if (result.GetResultType() != NumberType.BOOL) {
                    ErrorHandler.ErrorInExpression(CactusError.CONDITION_ERROR, ctx.loopCond(), result.GetResultType());
                }
            });

        WhileLoop whileLoop = new(loopCond, _currentBlock);

        //Visit inner block
        var storeCurrentBlock = _currentBlock;
        _currentBlock = whileLoop.Body;
        var result = base.VisitCodeBody(ctx.codeBody());

        _currentBlock = storeCurrentBlock;
        _currentBlock.AddStatement(whileLoop);
        return result;
    }

    public override StatusCode VisitForLoop(GrammarParser.ForLoopContext ctx) {
        VarDclStatement loopDcl = new(_currentBlock);
        FillVarDclStatement(ctx.loopDecl().varDcl(), loopDcl);

        var loopCondExprCtx = ctx.loopCond().expression();
        var loopCond = EvaluateExpression(
            loopCondExprCtx,
            whenErrExpr: DEFAULT,
            otherErrHandling: result => {
                if (result.GetResultType() != NumberType.BOOL) {
                    ErrorHandler.ErrorInExpression(CactusError.CONDITION_ERROR, ctx.loopCond(), result.GetResultType());
                }
            });

        var endExprCtx = ctx.endExp().expression();
        var endExp = EvaluateExpression(endExprCtx);


        ForLoop forLoop = new(loopDcl, loopCond, endExp, _currentBlock);
        var storeCurrentBlock = _currentBlock;
        _currentBlock = forLoop.LoopBody;
        var result = base.VisitCodeBody(ctx.codeBody());

        _currentBlock = storeCurrentBlock;
        _currentBlock.AddStatement(forLoop);
        return result;
    }

    #endregion

    #endregion


    private void DEFAULT(GP.ExpressionContext expCtx) =>
        ErrorHandler.ErrorInExpression(CactusError.EXPRESSION_ERROR, expCtx);

    private Expression EvaluateExpression(
        GP.ExpressionContext expCtx,
        Action<GP.ExpressionContext>? whenErrExpr = null,
        Action<Expression>? otherErrHandling = null
    ) {
        var a = DEFAULT;
        var factory = CreateFactory(expCtx);
        var result = factory.EvaluateExpression();
        //Handle errors
        var resultType = result.GetResultType();
        if (resultType is ErrorType) {
            if (whenErrExpr is null)
                DEFAULT(expCtx);
            else
                whenErrExpr(expCtx);
        }

        if (otherErrHandling is not null) {
            otherErrHandling(result);
        }

        return result;
    }
}