using CactusLang.CodeGeneration.CCodeGenerator;
using CactusLang.Model;
using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Entities.IncludedFiles;
using CactusLang.Model.CodeStructure.Entities.Structs;
using CactusLang.Model.CodeStructure.FileContent;
using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Model.CodeStructure.Statements.ControlFlow;
using CactusLang.Model.Visitors;
using CactusLang.Tags;
using CactusLang.Util;

namespace CactusLang.CodeGeneration;

public class CactusFileGenerator : CodeModelVisitor<int> {
    public struct State {
    }

    public const string CCTS_STD_DIRECTORY = "ccts_std.h";

    public const string C_INCLUDE = "#include ";

    private GroveGenerator _groveGenerator;

    private State _state;
    public State GenState => _state;
    private readonly CactusGrove _cactusGrove;
    private readonly CactusFile _cactusFile;

    public CactusFile CactusFile => _cactusFile;
    //private CodeStore _codeStore;
    //public CodeStore CodeStore => _codeStore;

    private readonly string _fileName;
    public string FileName => _fileName;
    public string FilePath => $"{_outDirectory}/{_fileName}";
    private string _outDirectory;

    private NameResolver _resolver;

    #region Sourcefiles

    public CFile Source { get; private set; }
    private CFile Header { get; set; }

    #endregion

    public CactusFileGenerator(GroveGenerator groveGenerator, CactusFile cactusFile, string outputFileName) :
        base(1, 0) {
        _state = new();
        _cactusFile = cactusFile;
        _cactusGrove = cactusFile.Grove;

        var directory = Path.GetDirectoryName(outputFileName) ?? "";
        _outDirectory = $"{directory}/src";
        _fileName = Path.GetFileNameWithoutExtension(outputFileName);

        string headerPath = $"{_outDirectory}/{_fileName}.h";
        string sourcePath = $"{_outDirectory}/{_fileName}.c";

        Source = new CFile(sourcePath);
        Header = new CFile(headerPath);


        _resolver = new(cactusFile);

        _groveGenerator = groveGenerator;
    }

    public void GenerateCode() {
        InitHeader();
        InitSource();

        VisitCodeFile(_cactusFile);
    }

    public void GenerateEntryFileCode() {
        InitHeader();
        InitSource();

        InitEntryPoint();

        VisitCodeFile(_cactusFile);
    }

    public void PrintCode() {
        Console.WriteLine($"-----------------HEADER {_fileName}.h-----------------");
        Header.Print();


        Console.WriteLine($"-----------------SOURCE {_fileName}.c-----------------");
        Source.Print();
    }

    private void InitSource() {
        Source.AddLineTop($"#include \"{_fileName}.h\"");
    }

    private void InitEntryPoint() {
        Source.AddAsMultipleLines(
            $$"""

              i32 main() {
                  return {{_resolver.GetFileFunc(_cactusGrove.GetEntryPoint())}}();
              }

              """
        );
    }

    private void InitHeader() {
        string headerGuard = _resolver.GetFileAsHeaderGuard(_cactusFile);

        Header.AddAsMultipleLines(
            CodeScope.Location.TOP,
            $$"""
              #ifndef {{headerGuard}}
              #define {{headerGuard}}

              #include "ccts_std.h"
              //Type table for objects
              #include "type_table.h"
              """
        );

        Header.AddLineBot($"#endif");
    }

    #region Preprocessing

    protected override int VisitIncludedFile(IncludedFile includedFile) {
        Header.AddLineTop($"#include \"{_resolver.GetFileAsHeaderFile(includedFile.CactusFile)}\"");
        return 1;
    }

    #endregion

    #region FileScopeHeader

    protected override int VisitFileFunction(FileFunction fileFunction) {
        string header = _resolver.GetFileFunctionHeader(fileFunction);
        Header.AddLineTop(header + ";");

        Source.AddLine(header + " {");
        Source.StepIn();
        var res = base.VisitFileFunction(fileFunction);
        Source.StepOut();
        Source.AddLine("}");
        Source.AddLine("");
        return res;
    }

    #endregion

    #region Struct

    protected override int VisitFileStruct(CactusStruct cactusStruct) {
        Header.AddLineTop($"typedef struct {{");

        //Switch the order around, so we can close the struct, then declare the functions
        Header.StepInTop();

        foreach (var field in cactusStruct.Fields.GetFields())
            VisitStructField(field);

        Header.StepOutTop();
        Header.AddLineBot();
        Header.AddLineBot();

        Header.AddLineTop($"}} {_resolver.GetMemberHolderName(cactusStruct)};");

        foreach (var func in cactusStruct.Methods.GetFunctions())
            VisitStructMethod(func);

        return 1;
    }

    protected override int VisitStructField(StructField sF) {
        Header.AddLineTop($"{_resolver.GetBaseType(sF.Type)} {sF.Name};");
        return base.VisitStructField(sF);
    }

    protected override int VisitStructMethod(StructMethod method) {
        //Generate method
        string header = _resolver.GetStructMethodHeader(method);
        Header.AddLine(header + ";");

        Source.AddLine(header + " {");
        Source.StepIn();
        var res = base.VisitStructMethod(method);
        Source.StepOut();
        Source.AddLine("}");
        Source.AddLine("");

        return res;
    }

    #endregion

    #region Statements

    protected override int VisitExpressionStatement(ExpressionStatement expressionStatement) {
        ExpressionCodeFactory factory = new(_resolver, expressionStatement.Expression, _state);

        string result = factory.Manufacture() + ";";
        Source.AddLine(result);
        return 1;
    }


    protected override int VisitReturnStatement(ReturnStatement returnStatement) {
        VisitStatementTags(returnStatement, t => { t.OnReturnStatement(returnStatement, this); });

        string retExpr = ExpressionCodeFactory.Manufacture(_resolver, returnStatement.Expression, _state);
        Source.AddLine($"return  ({retExpr});");
        return 1;
    }


    protected override int VisitFreeStatement(FreeStatement freeStatement) {
        string freeExpr = ExpressionCodeFactory.Manufacture(_resolver, freeStatement.Expression, _state);
        string result = $"free({freeExpr});";
        Source.AddLine(result);
        return 1;
    }

    protected override int VisitRawCCodeStatement(RawCCodeStatement rawCCodeStatement) {
        string[] lines = rawCCodeStatement.RawCode.Split('\n');
        foreach (string ln in lines)
            Source.AddLine(ln.TrimStart());
        return 1;
    }

    protected override int VisitVarDclStatement(VarDclStatement varDclStatement) {
        string result = GetVarDclString(varDclStatement) + ";";
        Source.AddLine(result);

        return 1;
    }


    #region Control Flow

    protected override int VisitIfStatement(IfStatement ifStatement) {
        Source.AddLine("if (" + ExpressionCodeFactory.Manufacture(_resolver, ifStatement.Condition) + ") {");

        Source.StepIn();
        var res = base.VisitCodeBlock(ifStatement.Body);
        Source.StepOut();

        if (ifStatement.Else != null)
            VisitElseBranch(ifStatement.Else);
        else
            Source.AddLine("} ");

        return res;
    }

    protected override int VisitElseBranch(ElseBranch elseBranch) {
        Source.Add("} else ");
        if (elseBranch.Condition != null)
            Source.Add("if (" + ExpressionCodeFactory.Manufacture(_resolver, elseBranch.Condition) + ") ");
        Source.AddLine("{");

        Source.StepIn();
        var res = base.VisitCodeBlock(elseBranch.Body);
        Source.StepOut();

        if (elseBranch.Else != null)
            VisitElseBranch(elseBranch.Else);
        else
            Source.AddLine("} ");

        return res;
    }

    protected override int VisitWhileLoop(WhileLoop whileLoop) {
        string loopTop = "while (" + ExpressionCodeFactory.Manufacture(_resolver, whileLoop.Condition) + ") {";
        Source.AddLine(loopTop);

        Source.StepIn();
        var res = base.VisitCodeBlock(whileLoop.Body);
        Source.StepOut();

        Source.AddLine("}");

        return res;
    }

    protected override int VisitForLoop(ForLoop forLoop) {
        var loopTop = "for (" + GetVarDclString(forLoop.LoopDcl)
                              + " ; " + ExpressionCodeFactory.Manufacture(_resolver, forLoop.Condition)
                              + " ; " + ExpressionCodeFactory.Manufacture(_resolver, forLoop.EndStatement) + ") {";
        Source.AddLine(loopTop);

        Source.StepIn();
        var res = base.VisitCodeBlock(forLoop.LoopBody);
        Source.StepOut();

        Source.AddLine("}");

        return res;
    }

    #endregion

    #endregion

    #region Tag visits

    private void VisitFileTags(CactusFile cactusFile, Action<Tag> action) {
        foreach (Tag tag in cactusFile.TagContainer.GetTags())
            action(tag);
    }

    private void VisitFunctionTags(CactusFunction func, Action<Tag> action) {
        VisitFileTags(func.CactusFile, action);

        foreach (Tag tag in func.TagContainer.GetTags())
            action(tag);
    }

    private void VisitStatementTags(Statement statement, Action<Tag> action) {
        VisitFunctionTags(statement.CodeBlock.Function, action);

        foreach (Tag tag in statement.TagContainer.GetTags())
            action.Invoke(tag);
    }

    #endregion

    #region IO

    public void GenerateFiles() {
        //Creating directory if doesnt exist
        Directory.CreateDirectory(_outDirectory);

        //Generate Header file
        Header.WriteToFile();
        _groveGenerator.HFilesGenerated.Add(Header);

        //Generate Source file
        Source.WriteToFile();
        _groveGenerator.CFilesGenerated.Add(Source);
    }


    private string GetVarDclString(VarDclStatement varDclStatement) {
        string result = "";
        result += $"{_resolver.GetBaseType(varDclStatement.VarType)} ";
        var dclBodies = varDclStatement.GetBodies();
        UtilFunc.SeparatedForEach(dclBodies,
            (body) => {
                result += body.Variable.CName;
                if (body.HasValue) {
                    string varVal = ExpressionCodeFactory.Manufacture(_resolver, body.Value!);
                    result += $" = {varVal}";
                }
            },
            (_, _) => result += NameResolver.PARAM_SEP
        );
        return result;
    }

    #endregion
}