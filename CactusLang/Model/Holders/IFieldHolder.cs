using CactusLang.Model.CodeStructure;

namespace CactusLang.Model.Holders;

public interface IFieldHolder<TField> where TField : ModelField
{
    public TField? GetField(string name);

    public List<TField> GetFields();
}