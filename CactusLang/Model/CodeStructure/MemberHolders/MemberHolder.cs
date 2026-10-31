using System.Collections;
using CactusLang.Model.Holders;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.MemberHolders;

public interface IMemberHolder : IFunctionHolder<MemberFunction>, IFieldHolder<MemberField> {
    public string Name { get; }

    public FileID ParentID { get; }

    public abstract BaseType MemberType { get; }
    
    
}