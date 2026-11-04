using System;
using CactusLang.Model;

namespace CactusLang.Tests;

public static class CctsTest {
	public static void TestHelloWorldLambda() {
		
		var vFile = new VirtualSourceFile("V/hello_world.ccts",
			"""
			i32 main() {
			    print_ln("Hello, world!");
			    
			    return 0;
			}

			void print_ln(ch08* str) $$C=> printf("%s\n", str);
			""");

		CactusGrove grove = new(vFile);
		
		CctsAssert.CompileRunMatchStdout(grove,
			"Hello, world!" + Environment.NewLine
		);
	}
}