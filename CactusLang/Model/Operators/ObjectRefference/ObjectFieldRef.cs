using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Entities;
using CactusLang.Model.Operators.Unary;
using CactusLang.Model.Types;

namespace CactusLang.Model.Operators.ObjectRefference;

public class ObjectFieldRef(IEntity referredStruct, CactusField mVariable, AccessType accessType) : UnaryOp(Side.RIGHT, mVariable.Name) {
    public IEntity Referred { get; } = referredStruct;
    public CactusField Variable { get; } = mVariable;

    public override CactusType Evaluate(CactusType val) => Variable.Symbol.Type;

    public AccessType AccessType { get; } = accessType;
}