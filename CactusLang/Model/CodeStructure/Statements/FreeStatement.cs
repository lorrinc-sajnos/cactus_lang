using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Statements;

public class FreeStatement : Statement {
    public Expression Expression { get; }

    public BaseType ReturnType => Expression.GetResultType();

    public FreeStatement(CodeBlock codeBlock, Expression expression) : base(codeBlock) {
        Expression = expression;
    }
}