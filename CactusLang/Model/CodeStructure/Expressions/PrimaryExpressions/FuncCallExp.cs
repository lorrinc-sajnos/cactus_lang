using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class FuncCallExp(ModelFunction function, List<Expression> parameters) : PrimaryExpression {
    public ModelFunction Function { get; } = function;

    public List<Expression> GetParameters() => parameters;

    public override bool IsLValue() => false;

    public override BaseType GetResultType() => Function.ReturnType;

    public override string ToString() =>  $"{Function}({string.Join(", ", GetParameters)})<{Function.ReturnType}>";
}