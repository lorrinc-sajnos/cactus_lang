using CactusLang.Model.Types;

namespace CactusLang.Model.Operators.Unary;

public class AddressOperator : UnaryOp {
    internal AddressOperator() : base(Side.LEFT, "&") { }

    public override CactusType Evaluate(CactusType val) {
        return val.GetPointer();
        // if (val is not PointerType ptr) 
        //     return ErrorType.ERROR;
        //
        // return ptr.PointsTo;
    }
}