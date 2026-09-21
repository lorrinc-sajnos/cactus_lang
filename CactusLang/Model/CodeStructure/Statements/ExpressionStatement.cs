using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Expressions;

namespace CactusLang.Model.CodeStructure.Statements;

public class ExpressionStatement : Statement {
    public  Expression Expression { get; }

    public ExpressionStatement(CodeBlock codeBlock, Expression expression): base(codeBlock) {
        Expression = expression;
    }
    
}