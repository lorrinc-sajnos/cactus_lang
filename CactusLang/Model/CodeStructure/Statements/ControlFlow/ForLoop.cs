using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Expressions;

namespace CactusLang.Model.CodeStructure.Statements.ControlFlow;

public class ForLoop(VarDclStatement loopDcl, Expression condition, Expression iter, CodeBlock parentBlock)
    : Statement(parentBlock)
{
    public VarDclStatement LoopDcl { get; } = loopDcl;

    public Expression Condition { get; } = condition;

    public Expression EndStatement { get; } = iter;

    public CodeBlock LoopBody { get; } = new(parentBlock.Function);
}