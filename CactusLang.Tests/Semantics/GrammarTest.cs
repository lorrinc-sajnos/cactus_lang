using System;
using CactusLang.Compiling.Codefiles;
using CactusLang.Model;
using CactusLang.Semantics;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests.Semantics;

[TestClass]
[TestSubject(typeof(SemanticFileAnalyzer))]
public class GrammarTest {
    

    [TestMethod]
    public void TestObjectReference() {
        CctsAssert.FileNoErrors("/ccts_lang/test_files/reference/simple_ref.ccts");
        //reference
        CctsAssert.FileNoErrors("/ccts_lang/test_files//reference/nested_ref.ccts");
    }

    [TestMethod]
    public void TestImplicitCastErrors() {
        CactusLangModel.InitCactusLang();
        
        string path = "test_files/type_system/impl_cast/test_impl_cast_fail_small.ccts";
        
        CactusGrove grove = new(path);
        Console.WriteLine($"Testing file: {path}");
        SemanticAnalyzer fileAnalyzer = new SemanticAnalyzer(grove);
        fileAnalyzer.AnalyzeGrove();
        fileAnalyzer.PrintErrors();
        //AssertNoErrors(analyzer);
        long test = 12345678901;
        Console.WriteLine($"Done!\n\n");
    }
}