using System;
using System.Numerics;
using CactusLang.Model;
using CactusLang.Semantics;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests.Semantics.Operators;

using INT = BigInteger; 

[TestClass]
[TestSubject(typeof(SemanticFileAnalyzer))]
public class TestIntOperators {

    record Operator<TResult> {
        public string Name;
        public Func<INT, INT, TResult> Function;
        
        public static Operator<INT> 
    }
    

    public static void TestOperator(INT lhs, INT rhs, string oper) {
        
        CctsAssert.CompileRunMatchStdout(new VirtualSourceFile(
                "OPERATOR.ccts",
                 string.Format("""
                i32 main() {{
                    i32 result = {0} {2} {1};

                    print_i32(result);
                    
                    return 0;
                }}
                void print_i32(i32 num) $$C=> printf("%d", num);
                """, lhs, rhs, oper)),
            "Hello, world!"
        );
    }
    
    
    
}