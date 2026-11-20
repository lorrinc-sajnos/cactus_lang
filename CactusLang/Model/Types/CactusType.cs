using CactusLang.Util;

namespace CactusLang.Model.Types;

public abstract class CactusType : IHaveName {
    public abstract string Name { get; }
    public abstract int Size { get; }

    public virtual bool IsInteger => false;
    public virtual bool IsNumber => false;
    public virtual bool IsUnsigned => false;
    
    /// <summary>
    /// Defines whether you can instantiate this type, or not
    /// </summary>
    public virtual bool IsAbstract => false;

    public virtual CactusType GetPointer() {
        return new PointerType(this);
    }

    public virtual bool CanBeUsedAs(CactusType superiorType) {
        return Equals(superiorType) || CanImplicitCastInto(superiorType);
    }

    public abstract bool CanImplicitCastInto(CactusType superiorType);
    public override string ToString() => Name;

    //public abstract string CName { get; }
}