using CactusLang.Model.Types;
using CactusLang.Model.Types.Abstract;

namespace CactusLang.Model.Operators;

public class AssignmentOperator(string format) : Operator(format, OperatorLvl.Assignment) {
    public override BaseType GetResultType(BaseType lhs, BaseType rhs) {
        if (rhs.CanBeUsedAs(lhs))
            return lhs;
        
        return ErrorType.ERROR;
    }
}