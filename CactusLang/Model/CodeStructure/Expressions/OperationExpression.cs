using CactusLang.Model.Operators;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions;

public class OperationExpression : Expression {
    private BaseType _resultType;

    public Operator Operator { get; }

    public Expression LeftExpression { get; }

    public Expression RightExpression { get; }

    public override bool IsLValue() => false;
    public override string ToString() => $"{LeftExpression} {Operator} {RightExpression}";
    public OperationExpression(Expression lhs, Operator operation, Expression rhs) {
        LeftExpression = lhs;
        RightExpression = rhs;
        Operator = operation;
        _resultType = Operator.Evaluate(LeftExpression.GetResultType(), RightExpression.GetResultType());
    }

    public override BaseType GetResultType() => _resultType;
    
    
}