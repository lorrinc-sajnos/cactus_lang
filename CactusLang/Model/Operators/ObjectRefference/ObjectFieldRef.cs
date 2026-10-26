using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.MemberHolders;
using CactusLang.Model.CodeStructure.MemberHolders.Structs;
using CactusLang.Model.Operators.Unary;
using CactusLang.Model.Types;

namespace CactusLang.Model.Operators.ObjectRefference;

public class ObjectFieldRef(IMemberHolder referredStruct, MemberField mField) : UnaryOp(Side.RIGHT, mField.Name) {
    public IMemberHolder Referred { get; } = referredStruct;
    public MemberField Field { get; } = mField;

    public override BaseType Evaluate(BaseType val) => Field.Symbol.Type;
}