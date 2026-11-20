using System.Linq.Expressions;
using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Entities.IncludedFiles;
using CactusLang.Model.CodeStructure.Entities.Structs;
using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.ObjectReference;
using CactusLang.Model.CodeStructure.FileContent;
using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Model.CodeStructure.Statements.ControlFlow;
using CactusLang.Model.Operators;
using CactusLang.Model.Operators.ObjectRefference;
using CactusLang.Model.Operators.Unary;
using Expression = CactusLang.Model.CodeStructure.Expressions.Expression;
using ObjectFieldRef = CactusLang.Model.Operators.ObjectRefference.ObjectFieldRef;
// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable UnusedParameter.Global

namespace CactusLang.Model.Visitors;

public class CodeModelVisitor<T> {
    protected CodeModelVisitor(T success, T failure) {
        _success = success;
        _fail = failure;
    }


    private readonly T _success;
    private readonly T _fail;

    protected virtual T AggregateResults(params T[] statuses) {
        foreach (var status in statuses) {
            if (status == null || status.Equals(_fail))
                return _fail;
        }

        return _success;
    }

    private T GetResult(List<T> statuses) => AggregateResults(statuses.ToArray());


    protected virtual T VisitCodeFile(CactusFile cactusFile) {
        var incResult = VisitIncludedFiles(cactusFile.IncludeManager.IncludedFiles);
        
        var strResult = VisitFileStructs(cactusFile.DeclStructStore.GetStructs());
        var varResult = VisitFileFields(cactusFile.Fields.GetFields());
        var funcResult = VisitFileFunctions(cactusFile.Functions.GetFunctions());

        return AggregateResults(strResult, varResult, funcResult);
    }

    #region Structs

    protected virtual T VisitFileStructs(List<CactusStruct> fileStructs) {
        var results = new List<T>();

        foreach (var fileStruct in fileStructs) {
            var strResult = VisitFileStruct(fileStruct);
            results.Add(strResult);
        }

        return GetResult(results);
    }

    protected virtual T VisitFileStruct(CactusStruct cactusStruct) {
        var results = new List<T>();

        foreach (var field in cactusStruct.Fields.GetFields()) {
            var result = VisitStructField(field);
            results.Add(result);
        }

        foreach (var func in cactusStruct.Methods.GetFunctions()) {
            var result = VisitStructMethod(func);
            results.Add(result);
        }

        return GetResult(results);
    }

    protected virtual T VisitStructField(StructField structField) {
        //Nothing to visit!
        return _success;
    }

    protected virtual T VisitStructMethod(StructMethod method) {
        return VisitCodeBlock(method.CodeBody);
    }

    #endregion

    #region File Variables

    protected virtual T VisitFileFields(List<FileVariable> fileVariables) {
        var results = new List<T>();
        foreach (var fileVariable in fileVariables) {
            var result = VisitFileField(fileVariable);
        }

        return GetResult(results);
    }

    protected virtual T VisitFileField(FileVariable fileVariable) {
        //Nothing to visit!
        return _success;
    }

    #endregion

    #region Included Files

    protected virtual T VisitIncludedFiles(List<IncludedFile> includedFiles) {
        var results = new List<T>();

        foreach (var includedFile in includedFiles) {
            var result = VisitIncludedFile(includedFile);
            results.Add(result);
        }

        return GetResult(results);
    }

    protected virtual T VisitIncludedFile(IncludedFile includedFile) {
        //Nothing to visit (?)
        return _success;
    }
    #endregion
    
    #region Methods

    protected virtual T VisitFileFunctions(List<FileFunction> fileFunctions) {
        var results = new List<T>();

        foreach (var fileFunction in fileFunctions) {
            var result = VisitFileFunction(fileFunction);
            results.Add(result);
        }

        return GetResult(results);
    }

    protected virtual T VisitFileFunction(FileFunction fileFunction) {
        return VisitCodeBlock(fileFunction.CodeBody);
    }

    protected virtual T VisitCodeBlock(CodeBlock codeBlock) {
        var results = new List<T>();
        foreach (var statement in codeBlock.Statements) {
            var result = VisitStatement(statement);
        }

        return GetResult(results);
    }

    #endregion

    #region Statements

    protected virtual T VisitStatement(Statement statement) {
        switch (statement) {
            default:
                throw new NotImplementedException($"Statement {statement} is not implemented");
            //return VisitStatus.FAILURE;

            case ExpressionStatement expressionStatement:
                return VisitExpressionStatement(expressionStatement);

            case VarDclStatement varDclStatement:
                return VisitVarDclStatement(varDclStatement);

            case ReturnStatement returnStatement:
                return VisitReturnStatement(returnStatement);

            case FreeStatement freeStatement:
                return VisitFreeStatement(freeStatement);

            case RawCCodeStatement rawCCodeStatement:
                return VisitRawCCodeStatement(rawCCodeStatement);

            //Control Flow
            case IfStatement ifStatement:
                return VisitIfStatement(ifStatement);
            
            case WhileLoop whileLoop:
                return VisitWhileLoop(whileLoop);
            
            case ForLoop forLoop:
                return VisitForLoop(forLoop);

        }
    }

    protected virtual T VisitReturnStatement(ReturnStatement returnStatement) {
        return VisitExpression(returnStatement.Expression);
    }

    protected virtual T VisitVarDclStatement(VarDclStatement varDclStatement) {
        var results = new List<T>();
        foreach (var body in varDclStatement.GetBodies()) {
            var result = VisitVarDclBody(body);
            results.Add(result);
        }

        return GetResult(results);
    }

    protected virtual T VisitVarDclBody(VarDclStatement.Body varBody) {
        if (varBody.HasValue)
            return VisitExpression(varBody.Value!);
        //else
        return _success;
    }

    protected virtual T VisitExpressionStatement(ExpressionStatement expressionStatement) {
        return VisitExpression(expressionStatement.Expression);
    }


    protected virtual T VisitFreeStatement(FreeStatement freeStatement) {
        return VisitExpression(freeStatement.Expression);
    }

    protected virtual T VisitRawCCodeStatement(RawCCodeStatement rawCCodeStatement) {
        return _success;
    }

    
    #region ControlFlow

    protected virtual T VisitIfStatement(IfStatement ifStatement)
    {
        var results = new  List<T>
        {
            VisitExpression(ifStatement.Condition),
            VisitCodeBlock(ifStatement.CodeBlock),
        };
        if (ifStatement.Else != null)
            results.Add(VisitElseBranch(ifStatement.Else));
        
        
        return GetResult(results);
    }
    protected virtual T VisitElseBranch(ElseBranch elseBranch)
    {
        var results = new List<T>();
        if (elseBranch.Condition != null)
            results.Add(VisitExpression(elseBranch.Condition));
        results.Add(VisitCodeBlock(elseBranch.Body));
        if (elseBranch.Else != null)
            results.Add(VisitElseBranch(elseBranch.Else));
        
        return GetResult(results);
    }

    protected virtual T VisitWhileLoop(WhileLoop whileLoop)
    {
        var results = new List<T>
        {
            VisitExpression(whileLoop.Condition),
            VisitCodeBlock(whileLoop.CodeBlock)
        };

        return GetResult(results);
    }
    
    protected virtual T VisitForLoop(ForLoop forLoop) {
        var results = new List<T>
        {
            VisitVarDclStatement(forLoop.LoopDcl),
            VisitExpression(forLoop.Condition),
            VisitExpression(forLoop.EndStatement),
            VisitCodeBlock(forLoop.CodeBlock)
        };

        return GetResult(results);
    }

    #endregion
    
    #endregion

    #region Expressions

    protected virtual T VisitExpression(Expression expression) {
        switch (expression) {
            default:
                throw new NotImplementedException($"Expression {expression} is not implemented");
            case OperationExpression operationExpression:
                return VisitOperationExpression(operationExpression);
            case PrimaryExpression primaryExpression:
                return VisitPrimaryExpression(primaryExpression);
        }
    }

    protected virtual T VisitOperationExpression(OperationExpression operationExpression) {
        var leftResult = VisitExpression(operationExpression.LeftExpression);
        var operatorResult = VisitOperator(operationExpression.Operator);
        var rightResult = VisitExpression(operationExpression.RightExpression);
        return AggregateResults(leftResult, rightResult);
    }

    protected virtual T VisitPrimaryExpression(PrimaryExpression primaryExpression) {
        T primExpResult;
        T opResult = _success;

        switch (primaryExpression) {
            default:
                throw new NotImplementedException($"Primary expression {primaryExpression} is not implemented");
            case UnOpPrimaryExpression unOpPrimaryExpression:
                if (unOpPrimaryExpression.UnaryOperator.OpSide == UnaryOp.Side.LEFT) {
                    primExpResult = VisitLeftUnOpPrimaryExpression(unOpPrimaryExpression);
                }
                else
                    primExpResult = VisitRightUnOpPrimaryExpression(unOpPrimaryExpression);

                break;

            case FuncCallExp funcCall:
                primExpResult = VisitFuncCallExpression(funcCall);
                break;
            case ParenthsExp parenthsExpression:
                primExpResult = VisitParenthsExpression(parenthsExpression);
                break;
            case VarRef varRef:
                primExpResult = VisitVarRefExpression(varRef);
                break;
            case LocalVarRef localVarRef:
                primExpResult = VisitLocalVarRefExpression(localVarRef);
                break;
            case LiteralExpression literalExpression:
                primExpResult = VisitLiteralExpression(literalExpression);
                break;
            case ObjectFieldRefExp objectFieldRefExp:
                primExpResult = VisitObjectFieldRefExp(objectFieldRefExp);
                break;
            case MethodCallExp objectFuncCallExp:
                primExpResult = VisitObjectFuncCallExp(objectFuncCallExp);
                break;
            case Alloc alloc:
                primExpResult = VisitAlloc(alloc);
                break;
            
            case IncludedFileRef incFileRef:
                primExpResult = VisitIncludedFileRef(incFileRef);
                break;
        }

        return AggregateResults(primExpResult, opResult);
    }

    #region PrimaryExpression

    protected virtual T VisitLeftUnOpPrimaryExpression(UnOpPrimaryExpression unOpPrimaryExpression) {
        var results = new List<T>
        {
            VisitUnaryOperator(unOpPrimaryExpression.UnaryOperator),
            VisitPrimaryExpression(unOpPrimaryExpression.PrimaryExpr)
        };

        return GetResult(results);
    }

    protected virtual T VisitRightUnOpPrimaryExpression(UnOpPrimaryExpression unOpPrimaryExpression) {
        var results = new List<T>
        {
            VisitPrimaryExpression(unOpPrimaryExpression.PrimaryExpr),
            VisitUnaryOperator(unOpPrimaryExpression.UnaryOperator)
        };

        return GetResult(results);
    }

    protected virtual T VisitFuncCallExpression(FuncCallExp funcCall) {
        var results = new List<T>();
        foreach (var parameter in funcCall.GetParameters()) {
            var result = VisitExpression(parameter);
            results.Add(result);
        }

        return GetResult(results);
    }

    protected virtual T VisitParenthsExpression(ParenthsExp parenthsExpression) {
        return VisitExpression(parenthsExpression.InnerExpression);
    }

    protected virtual T VisitVarRefExpression(VarRef varRef) {
        //Nothing to visit!
        return _success;
    }

    protected virtual T VisitLocalVarRefExpression(LocalVarRef structFieldRef) {
        //Nothing to visit!
        return _success;
    }

    protected virtual T VisitLiteralExpression(LiteralExpression literalExpression) {
        switch (literalExpression) {
            default:
                throw new NotImplementedException($"Literal expression {literalExpression} is not implemented");
            case FloatLiteral floatLiteral:
                return VisitFloatLiteralExpression(floatLiteral);
            case IntLiteral intLiteral:
                return VisitIntLiteralExpression(intLiteral);
        }
    }

    #region Object visiting

    protected virtual T VisitObjectFieldRefExp(ObjectFieldRefExp objectFieldRefExp) {
        return VisitPrimaryExpression(objectFieldRefExp.Object);
    }

    //protected virtual T VisitStructFieldRefExp()

    protected virtual T VisitObjectFuncCallExp(MethodCallExp methodCallExp) {
        var results = new List<T> { VisitPrimaryExpression(methodCallExp.Object) };
        results.AddRange(methodCallExp.GetParameters().Select(VisitExpression));

        return GetResult(results);
    }

    #endregion

    protected virtual T VisitAlloc(Alloc alloc) {
        return _success;
    }

    protected virtual T VisitIncludedFileRef(IncludedFileRef includedFileRef) {
        return _success;
    }

    #region Literal Expression

    protected virtual T VisitFloatLiteralExpression(FloatLiteral literalExpression) {
        //Nothing to visit!
        return _success;
    }

    protected virtual T VisitIntLiteralExpression(IntLiteral literalExpression) {
        //Nothing to visit!
        return _success;
    }

    protected virtual T VisitStrLiteralExpression(StringLiteral strLiteral) {
        return _success;
    }

    #endregion

    #endregion

    #endregion

    #region Operators

    #region Unary Operators

    protected virtual T VisitUnaryOperator(UnaryOp op) {
        switch (op) {
            default:
                throw new NotImplementedException($"Operator {op} is not implemented");
            case AddressOperator addressOperator:
                return VisitAddressOperator(addressOperator);
            case BaseUnaryOp checkUnaryOp:
                return VisitBaseUnaryOp(checkUnaryOp);
            case DereferenceOperator dereferenceOperator:
                return VisitDereferenceOperator(dereferenceOperator);

            case ObjectFieldRef objectFieldRef:
                return VisitObjectFieldRef(objectFieldRef);
            case ObjectFuncCall objectFuncCall:
                return VisitObjectFuncCall(objectFuncCall);
        }
    }

    protected virtual T VisitAddressOperator(AddressOperator addressOperator) {
        //Nothing to visit!
        return _success;
    }

    protected virtual T VisitBaseUnaryOp(BaseUnaryOp checkUnaryOp) {
        //Nothing to visit!
        return _success;
    }

    protected virtual T VisitDereferenceOperator(DereferenceOperator dereferenceOperator) {
        //Nothing to visit!
        return _success;
    }

    protected virtual T VisitObjectFieldRef(ObjectFieldRef objectFieldRef) {
        //Nothing to visit!
        return _success;
    }

    protected virtual T VisitObjectFuncCall(ObjectFuncCall objectFuncCall) {
        //Nothing to visit!
        return _success;
    }

    #endregion

    protected T VisitOperator(Operator op) {
        switch (op) {
            default:
                throw new NotImplementedException($"Operator {op} is not implemented");
            case BitShiftOperator bitShiftOperator:
                return VisitBitShiftOperator(bitShiftOperator);
            case CompOperator compOperator:
                return VisitCompOperator(compOperator);
            case IntOperator intOperator:
                return VisitIntOperator(intOperator);
            case NumberOperator mathOperator:
                return VisitMathOperator(mathOperator);
            case AssignmentOperator assignmentOperator:
                return VisitAssignmentOperator(assignmentOperator);
        }
    }

    protected virtual T VisitBitShiftOperator(BitShiftOperator op) {
        //Nothing to visit!
        return _success;
    }

    protected virtual T VisitCompOperator(CompOperator op) {
        //Nothing to visit!
        return _success;
    }

    protected virtual T VisitIntOperator(IntOperator op) {
        //Nothing to visit!
        return _success;
    }

    protected virtual T VisitMathOperator(NumberOperator op) {
        //Nothing to visit!
        return _success;
    }

    protected virtual T VisitAssignmentOperator(AssignmentOperator op) {
        //Nothing to visit!
        return _success;
    }

    #endregion
}