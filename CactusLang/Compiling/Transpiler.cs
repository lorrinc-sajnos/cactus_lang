using CactusLang.CodeGeneration;
using CactusLang.Compiling.Codefiles;
using CactusLang.Model;
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
    public SourceFileGenerator? CodeGenerator { get; private set; }
    public GccInstance? GccInstance { get; private set; }

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

        if (Grove.HasErrors()) {
            Console.WriteLine("-\tParsing failed");
            analysisFailed = true;
        }

        if (analysisFailed) {
            Grove.PrintErrors();
            Console.WriteLine("-\tC code generation terminated");
            return Result.FAIL;
        }


        Console.WriteLine("-\tParsing finished");

        GroveGenerator codeGenerator = new(grove);
        codeGenerator.GenerateSourceFiles();

        Console.WriteLine("-\tC code generation finished");

        GccInstance = new GccInstance(Grove, OutputFile, codeGenerator.CFilesGenerated);
        GccInstance.Compile();

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