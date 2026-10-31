using CactusLang.Model.Types;

namespace CactusLang.Model.Operators;

public class AssignmentOperator(string format) : Operator(format, OperatorLvl.Assignment) {
    public override BaseType Evaluate(BaseType lhs, BaseType rhs) {
        if (rhs.CanBeUsedAs(lhs))
            return lhs;
        
        return ErrorType.ERROR;
    }
}