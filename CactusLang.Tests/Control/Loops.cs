using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests.Control;


[TestClass]
[TestCategory(nameof(Loops))]
public class Loops {
    private static readonly string EOL = Environment.NewLine;
    
    [TestMethod]
    public void TestForLoop() {
        string result = "";
        int NUM = 67;

        for (int i = 0; i < NUM; i++) {
            result += i + EOL;
        }
        
        CctsAssert.CompileRunMatchStdout(new VirtualSourceFile(
                "ctrl/loop/for.ccts",
                $$"""
                i32 main() {
                    i32 num = {{NUM}};
                    
                    for (i32 i = 0; i < num; i++) {
                        print_i32(i);
                    }
                    return 0;
                }
                void print_i32(i32 num) $$C=> printf("%d\n", num);
                """),
            result
        );
    }
    
    [TestMethod]
    public void TestWhileLoop() {
        string result = "";
        int NUM = 67;
        
        int cnt = 0;
        while( cnt < NUM) {
            result += cnt + EOL;
            cnt++;
        }
        
        CctsAssert.CompileRunMatchStdout(new VirtualSourceFile(
                "ctrl/loop/for.ccts",
                $$"""
                  i32 main() {
                      i32 num = {{NUM}};
                      i32 cnt = 0;
                      
                      while (cnt < num) {
                          print_i32(cnt);
                          cnt++;
                      }
                      return 0;
                  }
                  void print_i32(i32 num) $$C=> printf("%d\n", num);
                  """),
            result
        );
    }
}