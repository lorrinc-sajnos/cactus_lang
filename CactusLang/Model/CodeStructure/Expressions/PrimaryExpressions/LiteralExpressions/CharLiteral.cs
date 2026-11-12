using CactusLang.Model.Types;
using CactusLang.Model.Types.PrimitiveTypes;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;

public class CharLiteral :  LiteralExpression {
    public char CharValue {get; private set;}
    public override string Value => $"\'{CharValue}\'";
    NumberType  _type;   //TODO float literal!
    public NumberType Type => _type;

    public CharLiteral(NumberType type, string value) {
        CharValue = value[0];
        _type = type;
    }

    public override BaseType GetResultType() => _type;
}