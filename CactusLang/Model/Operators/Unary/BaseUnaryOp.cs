using CactusLang.Model.Types;
using CactusLang.Model.Types.Abstract;
using CactusLang.Model.Types.PrimitiveTypes;

namespace CactusLang.Model.Operators.Unary;

public class BaseUnaryOp(UnaryOp.Side side, string format, Func<NumberType, bool> check) 
    : UnaryOp(side, format) {
    
    public override BaseType Evaluate(BaseType val) {
        if (val is not NumberType prim)
            return ErrorType.ERROR;

        //If fails check
        if (!check(prim))
            return ErrorType.ERROR;
        
        return prim;
    }
}