using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests.Control;

[TestClass]
[TestCategory(nameof(IfElse))]
public class IfElse {
    private static readonly string EOL = Environment.NewLine;

    [TestMethod]
    public void TestIfBranchTrue() =>
        CctsAssert.CompileRunMatchStdout(new VirtualSourceFile(
                "ctrl/if_else/if_A.ccts",
                """
                i32 main() {
                    i32 num = 123;
                    
                    if( num > 10 ){
                        print_ln("Big, if true!");
                    }
                    print_ln("Anyways...");
                    return 0;
                }
                void print_ln(ch08* str) $$C=> printf("%s\n", str);
                """),
            "Big, if true!" + EOL + "Anyways..." + EOL
        );

    [TestMethod]
    public void TestIfBranchFalse() =>
        CctsAssert.CompileRunMatchStdout(new VirtualSourceFile(
                "ctrl/if_B.ccts",
                """
                i32 main() {
                    i32 num = 123;
                    
                    if( num > 100000 ){
                        print_ln("Big, if true!");
                    }
                    print_ln("Anyways...");
                    return 0;
                }
                void print_ln(ch08* str) $$C=> printf("%s\n", str);
                """),
            "Anyways..." + EOL
        );

    [TestMethod]
    public void TestElseBranch() =>
        CctsAssert.CompileRunMatchStdout(new VirtualSourceFile(
                "ctrl/if_C.ccts",
                """
                i32 main() {
                    i32 num = 123;
                    
                    if( num > 100000 ){
                        print_ln("Big, if true!");
                    } else {
                        print_ln("Else??");
                    }
                    print_ln("Anyways...");
                    return 0;
                }
                void print_ln(ch08* str) $$C=> printf("%s\n", str);
                """),
            "Else??" + EOL + "Anyways..." + EOL
        );

    [TestMethod]
    public void TestElseIfBranch() =>
        CctsAssert.CompileRunMatchStdout(new VirtualSourceFile(
                "ctrl/if_D.ccts",
                """
                i32 main() {
                    i32 num = 123;
                    
                    if( num > 100000 ){
                        print_ln("Big, if true!");
                    } else if (num > 100){
                        print_ln("Elif!!");
                    }
                    print_ln("Anyways...");
                    return 0;
                }
                void print_ln(ch08* str) $$C=> printf("%s\n", str);
                """),
            "Elif!!" + EOL + "Anyways..." + EOL
        );

    [TestMethod]
    public void TestLastElse() =>
        CctsAssert.CompileRunMatchStdout(new VirtualSourceFile(
                "ctrl/if_D.ccts",
                """
                i32 main() {
                    i32 num = 123;
                    
                    if( num > 100000 ){
                        print_ln("Big, if true!");
                    } else if (num > 1000){
                        print_ln("Elif!!");
                    } else {
                        print_ln("Last :(");
                    }
                    print_ln("Anyways...");
                    return 0;
                }
                void print_ln(ch08* str) $$C=> printf("%s\n", str);
                """),
            "Last :(" + EOL + "Anyways..." + EOL
        );
}