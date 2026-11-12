using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CactusLang.Model;
using CactusLang.Model.Operators;
using CactusLang.Model.Types;
using CactusLang.Model.Types.PrimitiveTypes;
using CactusLang.Semantics;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests.Semantics.Operators;

using INT = BigInteger;

[TestClass]
[TestSubject(typeof(SemanticFileAnalyzer))]
public class TestIntOperators
{
	public static readonly string EOL = Environment.NewLine;
	
	public class TestIntOperator(string name, Operator cctsOperator, Func<INT, INT, INT> function) {
		public readonly string Name = name;
		public readonly Operator CactusOperator = cctsOperator;
		public readonly Func<INT, INT, INT> Function = function;

		public static TestIntOperator[] IntOperators = [
			new("add", Operator.ADD, (x,y)=>x+y),
			new("neg", Operator.SUB, (x,y)=>x-y),
			new("mult", Operator.MULT,(x,y)=>x*y),
			new("div", Operator.DIV, (x,y)=>x/y),
            
			new("rem", Operator.REMAINDER, (x,y)=> x%y),
            
			new("bit_and",Operator.BIT_AND, (x,y)=>x&y),
			new("bit_or", Operator.BIT_OR, (x,y)=> x|y),
			new("bit_xor", Operator.BIT_XOR, (x,y)=>x^y),
            
			new("bit_shift_left", Operator.BIT_LEFT_SHIFT, (x,y)=>x<<(int)y),
			new("bit_shit_right", Operator.BIT_RIGHT_SHIFT, (x,y)=>x>>(int)y)
		];
	}
	
	public static void CompileRunIntOperator(
		NumberType type_A, INT A, 
		NumberType type_B, INT B, 
		TestIntOperator oper) 
	{
		var resultType = oper.CactusOperator.GetResultType(type_A, type_B);

		//AB
		var resultAB = oper.Function.Invoke(A, B);
		Console.WriteLine("TESTING (" +
		                  $"{type_A.Name.ToUpper()}) {A} " +
		                  $"{oper.CactusOperator.GetId()} " +
		                  $"({type_B.Name.ToUpper()}) {B}\n" +
		                  $"Expected result: {resultType} {resultAB}\n");
		
		//BA
		var resultBA = oper.Function.Invoke(B, A);
		Console.WriteLine("TESTING (" +
		                  $"{type_B.Name.ToUpper()}) {B} " +
		                  $"{oper.CactusOperator.GetId()} " +
		                  $"({type_A.Name.ToUpper()}) {A}\n" +
		                  $"Expected result: {resultType} {resultBA}\n");


		string cStrFormat = "%d";
		if (resultType.Size > 4) {
			cStrFormat = resultType.IsUnsigned ? "%lu" : "%ld";
		}
		
		CctsAssert.CompileRunMatchStdout(new VirtualSourceFile(
				$"op_{oper.Name}.ccts",
				string.Format(
					"""
					i32 main() {{
					    {0} A = {1};
					    {2} B = {3};

					    {4} result_AB = A {5} B;
					    {4} result_BA = B {5} A;
					    
					    print_num(result_AB);
					    print_num(result_BA);
					    return 0;
					}}

					void print_num({4} num) $$C=> printf("{6}\n", num);      
					""",
					type_A.Name, A, 
					type_B.Name, B,
					resultType.Name, oper.CactusOperator.GetId(),
					cStrFormat
				)),
			$"{resultAB}{EOL}{resultBA}{EOL}"
		);
		Console.WriteLine("SUCCESS\n\n");
	}


	//[TestMethod]
	public void TestBasicOperators() {
		if (CactusLangModel.InitComplete) {
			Console.WriteLine("ALREADY INITIALIZED!");
		}
		else {
			CactusLangModel.InitCactusLang();
		}
		
		
		NumberType[] types = [
			NumberType.I08, NumberType.U08, NumberType.I16, NumberType.U16, NumberType.U32, NumberType.I32, NumberType.I64
		];
		
		
		NumberType[] unsigned_types = [
			NumberType.U08,  NumberType.U16, NumberType.U32,  NumberType.U64
		];
		
		foreach (var op in TestIntOperator.IntOperators) {
			foreach (var A_type in types) {
				foreach (var B_type in types) {
					INT A_value = A_type.IsUnsigned ? 11 : -12;
					INT B_value = B_type.IsUnsigned ? 44 : 57;
					
					CompileRunIntOperator(A_type, A_value, B_type, B_value, op);
				}
			}
		}
		
	}
	
}