using System;
using CactusLang.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests;

[TestClass]
[TestCategory("HelloWorld")]
public class HelloWorldTest {
	
	[TestMethod]
	public void TestHelloWorld() =>
		CctsAssert.CompileRunMatchStdout(
			new CactusGrove("../../../../ccts_lang/test_files/hello_world/hello_world.ccts"),
			"Hello, world!"+Environment.NewLine
		);
	[TestMethod]
	public void TestHelloWorldVirtualFile() =>
		CctsAssert.CompileRunMatchStdout(new VirtualSourceFile(
				"hello_world.ccts",
				"""
				i32 main() {
				    print("Hello, world!");
				    
				    return 0;
				}
				void print(ch08* str) $$C=> printf("%s", str);
				"""),
			"Hello, world!"
		);
}