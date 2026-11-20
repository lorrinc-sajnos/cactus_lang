using CactusLang.Model.CodeStructure;

using CactusLang.Model.CodeStructure.Entities;
using CactusLang.Model.Operators.Unary;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.Operators.ObjectRefference;

public class ObjectFuncCall(IEntity referred, CactusMethod memberFunc, AccessType accType)
    : UnaryOp(Side.RIGHT, memberFunc.Symbol.GetSignature().ToString()) {
    public IEntity Referred { get; } = referred;

    public CactusMethod Function { get; } = memberFunc;

    public CactusType ReturnType => Function.ReturnType;


    public override CactusType Evaluate(CactusType val) => ReturnType;

    public AccessType Access { get; } = accType;
}