using CactusLang.Model.CodeStructure.Statements;

namespace CactusLang.Model.CodeStructure.CodeBlocks;

public class CodeBlock(CactusFunction parentFunction)
{
    private readonly List<Statement> _statements = [];
    public CactusFunction Function { get; private set; } = parentFunction;
    public virtual List<Statement> Statements => _statements;

    public virtual void AddStatement(Statement statement) => _statements.Add(statement);
}