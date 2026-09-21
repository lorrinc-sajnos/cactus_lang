namespace CactusLang.CodeGeneration.CCodeGenerator;

public class CodeStore {
    public CodeScope Source { get; }

    public CodeScope Header { get; }

    public CodeStore() {
        Source = new CodeScope();
        Header = new CodeScope();
    }
}