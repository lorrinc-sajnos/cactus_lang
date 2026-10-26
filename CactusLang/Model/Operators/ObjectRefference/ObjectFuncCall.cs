using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.MemberHolders;
using CactusLang.Model.CodeStructure.MemberHolders.Structs;
using CactusLang.Model.Operators.Unary;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.Operators.ObjectRefference;

public class ObjectFuncCall(IMemberHolder referred, MemberFunction memberFunc)
    : UnaryOp(Side.RIGHT, memberFunc.Symbol.GetSignature().ToString()) {
    public IMemberHolder Referred { get; } = referred;

    public MemberFunction Function { get; } = memberFunc;

    public BaseType ReturnType => Function.ReturnType;


    public override BaseType Evaluate(BaseType val) => ReturnType;
}