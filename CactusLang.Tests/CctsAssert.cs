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
    
    public static void NoErrors(SemanticFileAnalyzer fileAnalyzer) =>
        Assert.IsEmpty(fileAnalyzer.ErrorHandler.GetErrors());


    public static void FileNoErrors(string path) {
        CactusLangModel.InitCactusLang();
        
        CactusGrove cactusGrove = new CactusGrove(path);
        Console.WriteLine($"Testing file: {path}");
        SemanticFileAnalyzer fileAnalyzer = new SemanticFileAnalyzer(cactusGrove.RootFile);
        fileAnalyzer.AnalyzeFile();
        fileAnalyzer.ErrorHandler.PrintErrors();
        NoErrors(fileAnalyzer);
        Console.WriteLine($"Done!\n\n");
    }

    public static void AssertTypeCompatibility(BaseType expected, BaseType actual) {
        Assert.IsTrue(actual.CanBeUsedAs(expected));
    }

    public static void CompileRunMatchStdout(string path, string expectedStdout) {
        CactusGrove cactusGrove = new CactusGrove(path);
        Transpiler transpiler = new Transpiler(cactusGrove);
        
        transpiler.Compile();
        
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