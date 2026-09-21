using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Expressions;

namespace CactusLang.Model.CodeStructure.Statements;

public class WhileLoop : Statement {
    public Expression Condition { get; }

    public CodeBlock Body { get; }

    public WhileLoop(CodeBlock func, Expression condition) :  base(func) {
        Condition = condition;
        Body = new CodeBlock(func.Function);
    }
}