using CactusLang.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests.Semantics.Operators;


[TestClass]
public class TestIntOperatorsLegacy {
    #region Basic operators
	[TestMethod]
    public void TestMult() {}

    [TestMethod]
    public void TestDiv() {
        CactusLangModel.InitCactusLang();
        CctsAssert.FileNoErrors(new CactusGrove("/../../../../ccts_lang/test_files/type_system/operators/int/div.ccts"));
    }

    [TestMethod]
    public void TestAdd() {
        CactusLangModel.InitCactusLang();
        CctsAssert.FileNoErrors(new CactusGrove("/../../../../ccts_lang/test_files/type_system/operators/int/add.ccts"));
    }

    [TestMethod]
    public void TestSub() {
        CactusLangModel.InitCactusLang();
        CctsAssert.FileNoErrors(new CactusGrove("/../../../../ccts_lang/test_files/type_system/operators/int/sub.ccts"));
    }

    [TestMethod]
    public void TestRem() {
        CactusLangModel.InitCactusLang();
        CctsAssert.FileNoErrors(new CactusGrove("/../../../../ccts_lang/test_files/type_system/operators/int/rem.ccts"));
    }
    #endregion

    #region Bitwise
    [TestMethod]
    public void TestBitAnd() {
        CactusLangModel.InitCactusLang();
        CctsAssert.FileNoErrors(new CactusGrove("/../../../../ccts_lang/test_files/type_system/operators/int/bit_and.ccts"));
    }

    [TestMethod]
    public void TestBitOr() {
        CactusLangModel.InitCactusLang();
        CctsAssert.FileNoErrors(new CactusGrove("/../../../../ccts_lang/test_files/type_system/operators/int/bit_or.ccts"));
    }

    [TestMethod]
    public void TestBitXor() {
        CactusLangModel.InitCactusLang();
        CctsAssert.FileNoErrors(new CactusGrove("/../../../../ccts_lang/test_files/type_system/operators/int/bit_xor.ccts"));
    }

    [TestMethod]
    public void TestBitShiftLeft() {
        CactusLangModel.InitCactusLang();
        CctsAssert.FileNoErrors(new CactusGrove("/../../../../ccts_lang/test_files/type_system/operators/int/bit_shift_l.ccts"));
    }

    [TestMethod]
    public void TestBitShiftRight() {
        CactusLangModel.InitCactusLang();
        CctsAssert.FileNoErrors(new CactusGrove("/../../../../ccts_lang/test_files/type_system/operators/int/bit_shift_r.ccts"));
    }
    #endregion

    [TestMethod]
    public void TestEquals() {
        CactusLangModel.InitCactusLang();
        CctsAssert.FileNoErrors(new CactusGrove("/../../../../ccts_lang/test_files/type_system/operators/int/eq.ccts"));
    }

    [TestMethod]
    public void TestNotEquals() {
        CactusLangModel.InitCactusLang();
        CctsAssert.FileNoErrors(new CactusGrove("/../../../../ccts_lang/test_files/type_system/operators/int/not_eq.ccts"));
    }

    [TestMethod]
    public void TestGreaterThan() {
        CactusLangModel.InitCactusLang();
        CctsAssert.FileNoErrors(new CactusGrove("/../../../../ccts_lang/test_files/type_system/operators/int/greater_than.ccts"));
    }

    [TestMethod]
    public void TestLessThan() {
        CactusLangModel.InitCactusLang();
        CctsAssert.FileNoErrors(new CactusGrove("/../../../../ccts_lang/test_files/type_system/operators/int/less_than.ccts"));
    }

    [TestMethod]
    public void TestGreaterThanOrEqual() {
        CactusLangModel.InitCactusLang();
        CctsAssert.FileNoErrors(new CactusGrove("/../../../../ccts_lang/test_files/type_system/operators/int/greater_than_eq.ccts"));
    }

    [TestMethod]
    public void TestLessThanOrEqual() {
        CactusLangModel.InitCactusLang();
        CctsAssert.FileNoErrors(new CactusGrove("/../../../../ccts_lang/test_files/type_system/operators/int/less_than_eq.ccts"));
    }
}