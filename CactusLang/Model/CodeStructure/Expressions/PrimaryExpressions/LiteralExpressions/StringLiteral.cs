using CactusLang.Model.Types;
using CactusLang.Model.Types.PrimitiveTypes;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;

public class StringLiteral : LiteralExpression {
    private string _value;
    public override BaseType GetResultType() => NumberType.CH08.GetPointer();
    public override string Value => $"{_value}";

    public StringLiteral(string value) {
        _value = value;
    }
}