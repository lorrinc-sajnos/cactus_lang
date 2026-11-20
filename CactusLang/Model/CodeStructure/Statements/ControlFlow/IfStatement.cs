using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Expressions;

namespace CactusLang.Model.CodeStructure.Statements.ControlFlow;

public interface IElseBranchOwner
{
    public ElseBranch? Else { get; set; }
}

public class IfStatement(Expression condition, CodeBlock parentBlock, ElseBranch? elseBranch = null)
    : Statement(parentBlock), IElseBranchOwner
{
    public Expression Condition { get; } = condition;

    public CodeBlock Body { get; } = new(parentBlock.Function);

    public ElseBranch? Else { get; set;} = elseBranch;
}

public class ElseBranch(IElseBranchOwner parent, Expression? condition, CodeBlock parentBlock, ElseBranch? elseBranch=null) : IElseBranchOwner
{
    public IElseBranchOwner Parent { get; } = parent;
    public Expression? Condition { get; } = condition;

    public CodeBlock Body { get; } = new(parentBlock.Function);
    public ElseBranch? Else { get; set;} =  elseBranch;
}