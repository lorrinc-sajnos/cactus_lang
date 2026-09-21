using System;
using CactusLang.Model;
using CactusLang.Model.Codefiles;
using CactusLang.Semantics;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests.Semantics;

[TestClass]
[TestSubject(typeof(SemanticAnalyzer))]
public class GrammarTest {
    

    [TestMethod]
    public void TestObjectReference() {
        CactusLangModel.InitCactusLang();
        
        CctsAssert.FileNoErrors("/ccts_lang/test_files/reference/simple_ref.ccts");
        //reference
        CctsAssert.FileNoErrors("/ccts_lang/test_files//reference/nested_ref.ccts");
    }

    [TestMethod]
    public void TestImplicitCastErrors() {
        CactusLangModel.InitCactusLang();
        
        string path = "test_files/type_system/impl_cast/test_impl_cast_fail_small.ccts";
        
        CodeSourceFile file = new CodeSourceFile(path);
        Console.WriteLine($"Testing file: {path}");
        SemanticAnalyzer analyzer = new SemanticAnalyzer(file);
        analyzer.Analyze();
        analyzer.ErrorHandler.PrintErrors();
        //AssertNoErrors(analyzer);
        long test = 12345678901;
        Console.WriteLine($"Done!\n\n");
    }
}