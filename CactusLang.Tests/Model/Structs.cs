using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests.Model;

[TestClass]
public class Structs
{
    
    [TestMethod]
    public void TestSimpleStruct() =>
        CctsAssert.CompileRunMatchStdout(new VirtualSourceFile(
                "struct/test.ccts",
                """
                struct Foo{
                    ch08* str;
                    
                    i32 num;
                }
                
                void print(ch08* str) $$C=> printf("%s", str);
                
                i32 main() {
                    Foo foo;
                    foo:str = "Hello, world!";
                    foo:num = 123;
                    
                    print(foo:str);
                    
                    //Foo* foo2 = alloc Foo;
                   // foo2.num = -1;
                    
                    
                    return 0;
                }
                """),
            "Hello, world!"
        );
}