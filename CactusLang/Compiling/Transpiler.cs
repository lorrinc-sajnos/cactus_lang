using CactusLang.CodeGeneration;
using CactusLang.Compiling.Codefiles;
using CactusLang.Semantics;

namespace CactusLang.Compiling;

public class Transpiler(CactusGrove grove) {
    public enum Result {
        SUCCESS,
        FAIL,
        WARNING
    }


    public SemanticAnalyzer SemanticAnalyzer = new(grove);
    public string InputFilepath { get; } = grove.RootFilePath;
    public string OutputFile { get; } = grove.OutputFilePath;

    //Transpiler 
    public CactusGrove Grove { get; } = grove;
    public CodeGenerator? CodeGenerator { get; private set; }
    public GccInstance? GccInstance { get; private set; }


    public void GenerateCCode() {
        var mainCodeGenerator = new CodeGenerator(Grove, Grove.RootFile, Grove.RootFile.SourceFile.Path);

        mainCodeGenerator.GenerateEntryFileCode();
        mainCodeGenerator.GenerateFiles();

        var other_files = Grove.GetFiles().ToList();
        other_files.Remove(Grove.RootFile);
        foreach (var file in other_files) {
            var codeGenerator = new CodeGenerator(Grove, file, file.SourceFile.Path);

            codeGenerator.GenerateCode();
            codeGenerator.GenerateFiles();
        }
    }

    public void GccCompilation() {
        //TEMP
        GccInstance = new GccInstance(Grove, OutputFile);
        GccInstance.Compile();
    }

    public Result Compile() {
        Console.WriteLine($"-\tBegin parsing file {InputFilepath}");


        bool analysisFailed = false;
        try {
            SemanticAnalyzer.AnalyzeGrove();
        }
        catch (Exception e) {
            Console.WriteLine("-\tInternal error during parsing:");
            Console.WriteLine(e.Message);
            //DEBUG
            throw;
        }

        if (SemanticAnalyzer.HasErrors) {
            Console.WriteLine("-\tParsing failed");
            analysisFailed = true;
        }

        if (analysisFailed) {
            SemanticAnalyzer.PrintErrors();
            Console.WriteLine("-\tC code generation terminated");
            return Result.FAIL;
        }


        Console.WriteLine("-\tParsing finished");

        GenerateCCode();

        Console.WriteLine("-\tC code generation finished");

        GccCompilation();

        if (GccInstance!.HasError) {
            Console.WriteLine("-\tGCC ERROR:");
            Console.WriteLine($"-\t{GccInstance!.ErrorStr}");
            Console.WriteLine($"-\tGCC Exit Code: {GccInstance!.ExitCode}");
            return Result.FAIL;
        }

        Console.WriteLine("-\tGCC compilation finished with no errors");

        return Result.SUCCESS;
    }
}