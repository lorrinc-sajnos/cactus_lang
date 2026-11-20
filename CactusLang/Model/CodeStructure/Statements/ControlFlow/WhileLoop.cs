using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Expressions;

namespace CactusLang.Model.CodeStructure.Statements.ControlFlow;

public class WhileLoop(Expression condition, CodeBlock parentBlock) : Statement(parentBlock)
{
    public Expression Condition { get; } = condition;

    public CodeBlock Body { get; } = new(parentBlock.Function);
}