using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Statements;

public class ReturnStatement : Statement {
    public Expression Expression { get; }

    public BaseType ReturnType => Expression.GetResultType();

    public ReturnStatement(CodeBlock codeBlock, Expression expression) : base(codeBlock) {
        Expression = expression;
    }
}