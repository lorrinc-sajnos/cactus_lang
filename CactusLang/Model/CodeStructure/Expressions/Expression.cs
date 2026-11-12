using CactusLang.Model.Types;
using CactusLang.Model.Types.Abstract;

namespace CactusLang.Model.CodeStructure.Expressions;

public abstract class Expression {
    public abstract BaseType GetResultType();
    public abstract bool IsLValue();
    public bool IsRValue() => !IsLValue();
    
    public class Error : Expression {
        public Error() { }
        public override BaseType GetResultType() => ErrorType.ERROR;
        public override bool IsLValue() => false;
        public override string ToString() => "EXPR_ERR";
    }

    //public new abstract string ToString();
    
}