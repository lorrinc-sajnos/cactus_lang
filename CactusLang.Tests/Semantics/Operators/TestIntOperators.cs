using System;
using System.Collections.Generic;
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
    class Operator<TResult> {
        public string Name;
        public string Code;
        public Func<INT, INT, TResult> Function;


        public Operator(string name, string code, Func<INT, INT, TResult> function) {
            Name = name;
            Code = code;
            Function = function;
            Operators.Add(this);
        }

        public static Operator<INT>[] IntOperators = [
            new("add", "+", (x,y)=>x+y),
            new("neg", "-", (x,y)=>x-y),
            new("mult", "*",(x,y)=>x*y),
            new("div", "/", (x,y)=>x/y),
            
            new("rem", "%", (x,y)=> x%y),
            
            new("bit_and","&", (x,y)=>x&y),
            new("bit_or", "|", (x,y)=> x|y),
            new("bit_xor", "^", (x,y)=>x^y),
            
            new("bit_shift_left", "<<", (x,y)=>x<<(int)y),
            new("bit_shit_right", ">>", (x,y)=>x>>(int)y)
        ];


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