using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Expressions;

namespace CactusLang.Model.CodeStructure.Statements;

public class IfStatement : Statement{
    public Expression Condition { get; }

    public CodeBlock Body { get; }

    public IfStatement(CodeBlock codeBlock, Expression condition, CodeBlock body) : base(codeBlock) {
        Condition = condition;
        Body = body;
    }
        
}