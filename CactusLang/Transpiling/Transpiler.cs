using CactusLang.CodeGeneration;
using CactusLang.Model.Codefiles;
using CactusLang.Semantics;

namespace CactusLang.Transpiling;

public class Transpiler {
	public string InputFile { get; private set; }
	public string OutputFile { get; private set; }

	//Transpiler 
	public CodeSourceFile CodeSourceFile { get; private set; }
	public SemanticAnalyzer? Analyzer { get; private set; }
	public CodeGenerator? CodeGenerator { get; private set; }
	public GccInstance? GccInstance { get; private set; }

	public Transpiler(string inputFile, string outputFile) {
		InputFile = inputFile;
		OutputFile = outputFile;
		
		CodeSourceFile = new CodeSourceFile(InputFile);
	}

	public void SemanticAnalysis() {
		Analyzer = new SemanticAnalyzer(CodeSourceFile);
		Analyzer.Analyze();
	}

	public void GenerateCCode() {
		CodeGenerator = new CodeGenerator(Analyzer!.CodeFile, InputFile);
		
		CodeGenerator.GenerateCode();
		CodeGenerator.GenerateFiles();
	}

	public void GccCompilation() {
		GccInstance = new GccInstance(CodeGenerator!.FilePath, OutputFile);
		GccInstance.Compile();
	}

	public void Compile() {
		Console.WriteLine($"-\tBegin parsing file {InputFile}");
		
		SemanticAnalysis();
		
		Analyzer!.ErrorHandler.PrintErrors();
		if (Analyzer!.ErrorHandler.GetErrors().Count > 0) {
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