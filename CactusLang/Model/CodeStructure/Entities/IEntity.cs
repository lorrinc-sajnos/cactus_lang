using CactusLang.Model.Holders;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Entities;

public interface IEntity : IFunctionHolder<CactusMethod>, IFieldHolder<CactusField> {
    public string Name { get; }

    public FileID ParentID { get; }

    public abstract CactusType MemberType { get; }
}