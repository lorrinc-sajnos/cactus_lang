// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using CactusLang.CodeGeneration;
using CactusLang.Model;
using CactusLang.Model.Codefiles;
using CactusLang.Semantics;
using CactusLang.Transpiling;

namespace CactusLang;

class Program {
	public static void Main(string[] args) {
		if (args.Length <= 0) {
			Console.WriteLine("Input file must be provided!");
			return;
		}

		string inputFile = args[0];

		string srcDir = Path.GetDirectoryName(inputFile);

		string outputFile = $"{srcDir}/{Path.GetFileNameWithoutExtension(inputFile)}.out";
		if (args.Length > 1)
			outputFile = args[1];
		CactusLangModel.InitCactusLang();

		Transpiler transpilerInst = new(inputFile, outputFile);
		transpilerInst.Compile();


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