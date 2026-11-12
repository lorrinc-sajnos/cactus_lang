using CactusLang.Model.Types;
using CactusLang.Model.Types.Abstract;

namespace CactusLang.Model.Operators;

public class BitShiftOperator(string format, Operator.OperatorLvl level) : Operator(format, level) {
    public override BaseType GetResultType(BaseType valToShift, BaseType shiftCount) {
        if(!valToShift.IsInteger  || !shiftCount.IsInteger) return  ErrorType.ERROR;

        return valToShift;
    }
}