using CactusLang.Model.Operators;
using CactusLang.Model.Operators.Unary;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class UnOpPrimaryExpression(PrimaryExpression exp, UnaryOp op) : PrimaryExpression {
    public PrimaryExpression PrimaryExpression => exp;
    public UnaryOp UnaryOperation => op;


    public override BaseType GetResultType() => op.Evaluate(exp.GetResultType());
    public override bool IsLValue() => exp.IsLValue();
}