using CactusLang.Model.Types;

namespace CactusLang.Model.Operators.Unary;

public class BaseUnaryOp(UnaryOp.Side side, string format, Func<PrimitiveType, bool> check) 
    : UnaryOp(side, format) {
    
    public override BaseType Evaluate(BaseType val) {
        if (val is not PrimitiveType prim)
            return ErrorType.ERROR;

        //If fails check
        if (!check(prim))
            return ErrorType.ERROR;
        
        return prim;
    }
}