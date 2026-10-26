using CactusLang.Model.CodeStructure;

namespace CactusLang.Model;

public interface IFieldHolder<F> where F: ModelField{
    public F? GetField(string name);

    public List<F> GetFields();
}
