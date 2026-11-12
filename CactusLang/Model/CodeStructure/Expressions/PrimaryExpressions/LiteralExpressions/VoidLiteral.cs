using CactusLang.Model.Types;
using CactusLang.Model.Types.PrimitiveTypes;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;

public class VoidLiteral :  LiteralExpression {
    public override string Value => "void";

    public VoidLiteral() { }
    public override BaseType GetResultType() => NumberType.VOID;
}