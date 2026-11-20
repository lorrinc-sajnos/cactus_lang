using CactusLang.Model.CodeStructure.Statements;

namespace CactusLang.Model.CodeStructure.CodeBlocks;

public class CodeBlock(ModelFunction parentFunction)
{
    private readonly List<Statement> _statements = [];
    public ModelFunction Function { get; private set; } = parentFunction;
    public virtual List<Statement> Statements => _statements;

    public virtual void AddStatement(Statement statement) => _statements.Add(statement);
}