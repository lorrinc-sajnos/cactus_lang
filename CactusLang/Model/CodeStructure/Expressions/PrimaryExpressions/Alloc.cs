using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class Alloc : PrimaryExpression {
    private CactusType _type;
    public CactusType AllocType => _type;

    public Alloc(CactusType type) {
        _type = type;
    }
    public override CactusType GetResultType() => _type.GetPointer();

    public override bool IsLValue() => false;
}