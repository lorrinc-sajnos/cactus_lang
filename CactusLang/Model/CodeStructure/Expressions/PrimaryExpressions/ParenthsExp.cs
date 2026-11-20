using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class ParenthsExp(Expression innerExpression) : PrimaryExpression {
    public Expression InnerExpression { get; } = innerExpression;

    public override CactusType GetResultType() => InnerExpression.GetResultType();
    
    public override bool IsLValue() => InnerExpression.IsLValue();

    public override string ToString() => $"({InnerExpression})";
}