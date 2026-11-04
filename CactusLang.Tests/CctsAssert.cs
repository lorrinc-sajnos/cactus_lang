using System;
using System.Diagnostics;
using CactusLang.Compiling;
using CactusLang.Compiling.Codefiles;
using CactusLang.Model;
using CactusLang.Model.Types;
using CactusLang.Semantics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests;

public static class CctsAssert {

	
	public static void FileNoErrors(VirtualSourceFile vFile) => FileErrors(vFile, false);
	public static void FileNoErrors(CactusGrove cactusGrove) => FileErrors(cactusGrove, false);

	public static void FileErrors(VirtualSourceFile vFile, bool hasErrors = true) =>
		FileErrors(new CactusGrove(vFile), hasErrors);
	
	public static void FileErrors(CactusGrove cactusGrove, bool hasErrors = true) {
		CactusLangModel.InitCactusLang();

		Console.WriteLine($"Testing file: {cactusGrove.RootFilePath}");
		SemanticAnalyzer fileAnalyzer = new SemanticAnalyzer(cactusGrove);
		fileAnalyzer.AnalyzeGrove();
		
		Assert.AreEqual(cactusGrove.HasErrors(), hasErrors);
		Console.WriteLine($"Done!\n\n");
	}

	

	public static void AssertTypeCompatibility(BaseType expected, BaseType actual) {
		Assert.IsTrue(actual.CanBeUsedAs(expected));
	}
    
	public static void CompileRunMatchStdout(VirtualSourceFile vFile, string expectedStdout) {
		CactusGrove grove = new(vFile);
	        
		CompileRunMatchStdout(grove,expectedStdout);
	}

	public static void CompileRunMatchStdout(CactusGrove cactusGrove, string expectedStdout) {
		Transpiler transpiler = new Transpiler(cactusGrove);

		var result = transpiler.Compile();

		if (result != Transpiler.Result.SUCCESS) {
			Console.WriteLine("-Compilation failed, test failed... :(");
			Assert.Fail();
		}

		Process cctsProgram = new Process();
		cctsProgram.StartInfo.FileName = $"{transpiler.OutputFile}";

		// Redirect output so we can read it in C#
		cctsProgram.StartInfo.RedirectStandardOutput = true;
		cctsProgram.StartInfo.UseShellExecute = false;
		cctsProgram.StartInfo.CreateNoWindow = true;
		Console.WriteLine($"-RUNNING PROGRAM:");

		cctsProgram.Start();
		cctsProgram.WaitForExit();
		string stdout = cctsProgram.StandardOutput.ReadToEnd();

		Console.WriteLine(stdout);
		Console.WriteLine($"-PROGRAM RETURNED WITH: {cctsProgram.ExitCode}");

		Assert.AreEqual(expectedStdout, stdout);
	}

	//public static void 
}