using CactusLang.Model.Operators;
using CactusLang.Model.Operators.Unary;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class UnOpPrimaryExpression(PrimaryExpression expr, UnaryOp op) : PrimaryExpression {
    public PrimaryExpression PrimaryExpr { get; } = expr;
    public UnaryOp UnaryOperator { get; } = op;


    public override BaseType GetResultType() => UnaryOperator.Evaluate(PrimaryExpr.GetResultType());
    public override bool IsLValue() => PrimaryExpr.IsLValue();

    public override string ToString() {
        switch (UnaryOperator.OpSide) {
            case UnaryOp.Side.LEFT:
                return $"{UnaryOperator} {PrimaryExpr}";
            case UnaryOp.Side.RIGHT:
                return $"{PrimaryExpr} {UnaryOperator}";
            default:
                return $"OP:{UnaryOperator.OpSide}{PrimaryExpr}>";
        }
    }
}