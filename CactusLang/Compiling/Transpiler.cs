using CactusLang.CodeGeneration;
using CactusLang.Compiling.Codefiles;
using CactusLang.Semantics;

namespace CactusLang.Compiling;

public class Transpiler {
	public string InputFilepath { get; }
	public string OutputFile { get; }

	//Transpiler 
	public CactusGrove Grove { get; }
	public CodeGenerator? CodeGenerator { get; private set; }
	public GccInstance? GccInstance { get; private set; }
	
	public SemanticFileAnalyzer RootFileAnalyzer { get; private set; }

	public Transpiler(CactusGrove grove) {
		Grove = grove;
		InputFilepath = grove.RootFilePath;
		OutputFile = grove.OutputFilePath;
	}

	public void SemanticAnalysis() {
		RootFileAnalyzer = new SemanticFileAnalyzer(Grove.RootFile);
		
		RootFileAnalyzer.AnalyzeFile();
	}

	public void GenerateCCode() {
		foreach(var file in Grove.GetFiles()){
			var codeGenerator = new CodeGenerator(file, file.SourceFile.Path);

			codeGenerator.GenerateCode();
			codeGenerator.GenerateFiles();
		}
	}

	public void GccCompilation() {
		//TEMP
		GccInstance = new GccInstance(Grove, OutputFile);
		GccInstance.Compile();
	}

	public void Compile() {
		Console.WriteLine($"-\tBegin parsing file {InputFilepath}");

		SemanticAnalysis();

		RootFileAnalyzer!.ErrorHandler.PrintErrors();
		if (RootFileAnalyzer!.ErrorHandler.GetErrors().Count > 0) {
			Console.WriteLine("-\tParsing failed");
			Console.WriteLine("-\tC code generation terminated");

			return;
		}

		Console.WriteLine("-\tParsing finished");

		GenerateCCode();

		Console.WriteLine("-\tC code generation finished");

		GccCompilation();

		if (GccInstance!.HasError) {
			Console.WriteLine("-\tGCC ERROR:");
			Console.WriteLine($"-\t{GccInstance!.ErrorStr}");
			Console.WriteLine($"-\tGCC Exit Code: {GccInstance!.ExitCode}");
			return;
		}

		Console.WriteLine("-\tGCC compilation finished with no errors");
	}
}