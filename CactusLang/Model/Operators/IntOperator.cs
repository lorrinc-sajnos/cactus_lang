using CactusLang.Model.Types;
using CactusLang.Model.Types.Abstract;
using CactusLang.Model.Types.PrimitiveTypes;

namespace CactusLang.Model.Operators;

public class IntOperator(string format, Operator.OperatorLvl level) : Operator(format, level) {
    public override CactusType GetResultType(CactusType lhs, CactusType rhs) {
        if(!lhs.IsInteger  || !rhs.IsInteger) 
            return  ErrorType.ERROR;
        var A = lhs as NumberType;
        var B = rhs as NumberType;
        return NumberType.GetCommonFitType(A, B);
    }
}