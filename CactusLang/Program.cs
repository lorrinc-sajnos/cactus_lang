// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using CactusLang.CodeGeneration;
using CactusLang.Compiling;
using CactusLang.Model;
using CactusLang.Semantics;

namespace CactusLang;

class Program {
	public static void Main(string[] args) {
		if (args.Length <= 0) {
			Console.WriteLine("Input file must be provided!");
			return;
		}

		string inputFile = args[0];
		string srcDir = Path.GetDirectoryName(inputFile)!;
		string outputFile;
		if (args.Length > 1)
			outputFile = args[1];
		else
			outputFile = $"{srcDir}/{Path.GetFileNameWithoutExtension(inputFile)}.out";
			
		
		CactusLangModel.InitCactusLang();
		CactusGrove grove = new(inputFile);
		
		Transpiler transpilerInst = new(grove);
		transpilerInst.Compile();

		if (grove.RootFile.HasErrors()) {
			Console.WriteLine($"-COMPILE FAILED, CLOSING:");
			return;
		}

		Process cctsProgram = new Process();
		cctsProgram.StartInfo.FileName = $"{transpilerInst.OutputFile}";

		// Redirect output so we can read it in C#
		cctsProgram.StartInfo.RedirectStandardOutput = true;
		cctsProgram.StartInfo.UseShellExecute = false;
		cctsProgram.StartInfo.CreateNoWindow = true;
		Console.WriteLine($"-RUNNING PROGRAM:");

		cctsProgram.Start();
		cctsProgram.WaitForExit();

		Console.WriteLine(cctsProgram.StandardOutput.ReadToEnd());
		Console.WriteLine("-END OF PROGRAM, exit code {0}", cctsProgram.ExitCode);
	}
}
//Console.ReadKey();