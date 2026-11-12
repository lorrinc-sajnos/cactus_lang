using CactusLang.Model.Types;
using CactusLang.Model.Types.PrimitiveTypes;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;

public class FloatLiteral : LiteralExpression {
    NumberType _type; //TODO float literal!
    public NumberType Type => _type;
    public string FloatValue { get; private set; }
    public override string Value => FloatValue;

    public FloatLiteral(NumberType type, string? sign, string value) {
        FloatValue = sign + value;
        _type = type;
    }

    public override BaseType GetResultType() => _type;
}