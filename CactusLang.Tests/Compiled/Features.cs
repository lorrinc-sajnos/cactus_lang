using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests.Compiled;

[TestClass]
[TestSubject(typeof(Features))]
public class Features {
    
    [TestMethod]
    public void TestFunctionOverload() =>
        CctsAssert.CompileRunMatchStdout(new VirtualSourceFile(
                "features/func_overload.ccts",
                """
                void print(ch08* str) $$C=> printf("%s", str);

                void print(i32 num) $$C=> printf("%d", num);
                
                i32 main() {
                    print("Hello, world!");
                    print(123);
                    return 0;
                }
                """),
            "Hello, world!123"
        );
}