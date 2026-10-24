using CactusLang.CodeGeneration;
using CactusLang.Compiling.Codefiles;
using CactusLang.Semantics;

namespace CactusLang.Compiling;

public class Transpiler {
	public enum Result {
		SUCCESS,
		FAIL,
		WARNING
	}
	
	
	public SemanticAnalyzer SemanticAnalyzer;
	public string InputFilepath { get; }
	public string OutputFile { get; }

	//Transpiler 
	public CactusGrove Grove { get; }
	public CodeGenerator? CodeGenerator { get; private set; }
	public GccInstance? GccInstance { get; private set; }
	

	public Transpiler(CactusGrove grove) {
		Grove = grove;
		InputFilepath = grove.RootFilePath;
		OutputFile = grove.OutputFilePath;
		SemanticAnalyzer = new SemanticAnalyzer(grove);
	}

	public void GenerateCCode() {
		var mainCodeGenerator = new CodeGenerator(Grove, Grove.RootFile, Grove.RootFile.SourceFile.Path);

		mainCodeGenerator.GenerateEntryFileCode();
		mainCodeGenerator.GenerateFiles();

		var other_files = Grove.GetFiles().ToList();
		other_files.Remove(Grove.RootFile);
		foreach(var file in other_files){
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


		bool analysisFailed = true;
		try {
			SemanticAnalyzer.AnalyzeGrove();
		} catch (Exception e) {
			Console.WriteLine("-\tInternal error during parsing:");
			Console.WriteLine(e.Message);
			//DEBUG
			throw;
		}

		if (SemanticAnalyzer.HasErrors) {
			SemanticAnalyzer.PrintErrors();
			Console.WriteLine("-\tParsing failed");
			analysisFailed = true;
		}

		if (analysisFailed) {
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