using CactusLang.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests.Generation;

[TestClass]
[TestCategory(nameof(LinkingFiles))]
public class LinkingFiles {
	[TestMethod]
	public void TestSimpleLinking() =>
		CctsAssert.CompileRunMatchStdout(
			new CactusGrove("../../../../ccts_lang/test_files/link_files/simple/main.ccts"),
			"123456\n"
		);
}