using CactusLang.Model.Types;
using CactusLang.Model.Types.Abstract;
using CactusLang.Model.Types.PrimitiveTypes;

namespace CactusLang.Model.Operators;

public class NumberOperator(string format, Operator.OperatorLvl level) : Operator(format, level) {
	public override BaseType GetResultType(BaseType lhs, BaseType rhs) {
		if(!lhs.IsNumber  || !rhs.IsNumber) 
			return  ErrorType.ERROR;
		
		var A = lhs as NumberType;
		var B = rhs as NumberType;
		return NumberType.GetCommonFitType(A, B);
	}

}