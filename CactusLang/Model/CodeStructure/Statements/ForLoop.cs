using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Expressions;

namespace CactusLang.Model.CodeStructure.Statements;

public class ForLoop : Statement {
    public ForLoop(VarDclStatement loopDcl, Expression condition, Expression iter, CodeBlock parent) : base(parent) {
        LoopDcl = loopDcl;
        Condition = condition;
        EndStatement = iter;
        LoopBody = new CodeBlock(parent.Function);
    }

    public VarDclStatement LoopDcl { get; }

    public Expression Condition { get; }

    public Expression EndStatement { get; }

    public CodeBlock LoopBody { get; }
}