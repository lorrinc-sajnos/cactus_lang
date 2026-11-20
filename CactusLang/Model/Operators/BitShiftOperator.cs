using CactusLang.Model.Types;
using CactusLang.Model.Types.Abstract;

namespace CactusLang.Model.Operators;

public class BitShiftOperator(string format, Operator.OperatorLvl level) : Operator(format, level) {
    public override CactusType GetResultType(CactusType valToShift, CactusType shiftCount) {
        if(!valToShift.IsInteger  || !shiftCount.IsInteger) return  ErrorType.ERROR;

        return valToShift;
    }
}