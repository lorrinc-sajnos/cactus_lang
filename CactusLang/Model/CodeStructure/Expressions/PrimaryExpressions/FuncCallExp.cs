using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class FuncCallExp(CactusFunction function, List<Expression> parameters) : PrimaryExpression {
    public CactusFunction Function { get; } = function;

    public List<Expression> GetParameters() => parameters;

    public override bool IsLValue() => false;

    public override CactusType GetResultType() => Function.ReturnType;

    public override string ToString() =>  $"{Function}({string.Join(", ", GetParameters)})<{Function.ReturnType}>";
}