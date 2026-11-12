using CactusLang.Model.Types;
using CactusLang.Model.Types.Abstract;
using CactusLang.Model.Types.PrimitiveTypes;

namespace CactusLang.Model.Operators;

public class CompOperator : Operator {
    public CompOperator(string format, OperatorLvl level) : base(format, level) { }

    public override BaseType GetResultType(BaseType lhs, BaseType rhs) {
        if(!lhs.IsNumber  || !rhs.IsNumber){
            if (lhs.Equals(rhs))
                return NumberType.BOOL;
            return ErrorType.ERROR;
        }
        
        if (lhs.CanBeUsedAs(rhs) || rhs.CanBeUsedAs(lhs))
            return NumberType.BOOL;
        return ErrorType.ERROR;
    }
}