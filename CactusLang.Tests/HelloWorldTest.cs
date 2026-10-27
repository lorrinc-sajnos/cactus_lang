using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests;

[TestClass]
[TestCategory("HelloWorld")]
public class HelloWorldTest {
	
	[TestMethod]
	public void TestHelloWorld() =>
		CctsAssert.CompileRunMatchStdout(
			"../../../../ccts_lang/test_files/hello_world/hello_world.ccts",
			"Hello, world!\n"
		);
}