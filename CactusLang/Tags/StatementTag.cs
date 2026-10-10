using CactusLang.CodeGeneration;
using CactusLang.Model;
using CactusLang.Model.CodeStructure.Statements;

namespace CactusLang.Tags;

public abstract class Tag {
    public static string StaticID { get; }
    public abstract string Id { get; }
    
    
    //...
    public virtual void OnDeclared(CactusFile cactusFile) { }
    //...
    public virtual void OnStatement(Statement statement, CodeGenerator codeGenerator) { }
    //...
    public virtual void OnReturnStatement(ReturnStatement statement, CodeGenerator codeGenerator) { }
    //...
}