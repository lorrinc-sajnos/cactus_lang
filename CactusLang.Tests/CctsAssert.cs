using System;
using CactusLang.Model.Codefiles;
using CactusLang.Model.Types;
using CactusLang.Semantics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests;

public static class CctsAssert {
    
    public static void NoErrors(SemanticAnalyzer analyzer) =>
        Assert.IsEmpty(analyzer.ErrorHandler.GetErrors());


    public static void FileNoErrors(string path) {
        CodeSourceFile file = new CodeSourceFile(path);
        Console.WriteLine($"Testing file: {path}");
        SemanticAnalyzer analyzer = new SemanticAnalyzer(file);
        analyzer.Analyze();
        analyzer.ErrorHandler.PrintErrors();
        NoErrors(analyzer);
        Console.WriteLine($"Done!\n\n");
    }

    public static void AssertTypeCompatibility(BaseType expected, BaseType actual) {
        Assert.IsTrue(actual.CanBeUsedAs(expected));
    }
    
    //public static void 
}