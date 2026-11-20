using CactusLang.Model.CodeStructure;

namespace CactusLang.Model.Holders;

public interface IFieldHolder<TField> where TField : CactusVariable
{
    public TField? GetField(string name);

    public List<TField> GetFields();
}