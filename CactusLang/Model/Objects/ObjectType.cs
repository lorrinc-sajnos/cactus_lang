using CactusLang.Model.Types;

namespace CactusLang.Model.Objects;

public class ObjectType(BaseType instanceType) : BaseType
{
    public override string Name => $"object[{InstanceType.Name}]";
    public override int Size => -1;
    
    public BaseType InstanceType { get; } = instanceType;

    public override bool CanImplicitCastInto(BaseType superiorType)
    {
        throw new NotImplementedException();
    }
}