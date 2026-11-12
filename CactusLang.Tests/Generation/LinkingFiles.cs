using System;
using CactusLang.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests.Generation;

[TestClass]
[TestCategory(nameof(LinkingFiles))]
public class LinkingFiles {
	private static string EOL = Environment.NewLine;
	
	[TestMethod]
	public void TestSimpleLinking() =>
		CctsAssert.CompileRunMatchStdout(
			new CactusGrove("../../../../ccts_lang/test_files/link_files/simple/main.ccts"),
			"123456"
		);
	
	
	[TestMethod]
	public void TestComplexLinking() =>
		CctsAssert.CompileRunMatchStdout(
			new CactusGrove("../../../../ccts_lang/test_files/link_files/complex/main.ccts"),
			"123456" + EOL + 13287715 + EOL //13287715 is 0xCAC123 in decimal
		);
}