using CactusLang.Model;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Model.Types.PrimitiveTypes;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests.Model;

[TestClass]
[TestSubject(typeof(CactusLangModel))]
public class CactusLangModelTest {

    [TestMethod]
    public void InitLanguageTest() {
        //TODO test more stuff
        CactusLangModel.InitCactusLang();
        
        var primitives = NumberType.GetPrimitives();

        foreach (var primitive in primitives) {
            Assert.IsNotNull(primitive);
        }
    }
}