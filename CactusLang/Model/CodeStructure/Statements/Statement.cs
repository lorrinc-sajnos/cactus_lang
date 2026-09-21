using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Tags;

namespace CactusLang.Model.CodeStructure.Statements;

public abstract class Statement {
    public CodeBlock CodeBlock { get; }

    public TagContainer TagContainer { get; private set; }

    public Statement(CodeBlock parent) {
        CodeBlock = parent;
        TagContainer = new TagContainer();
    }
}