using CactusLang.Model.Types.Abstract;
using CactusLang.Model.Types.PrimitiveTypes;

namespace CactusLang.Model.Types;

public class PointerType : CactusType {
    private readonly CactusType _pointsTo;
    
    public CactusType PointsTo => _pointsTo;
    
    
    internal PointerType(CactusType pointsTo) {
        _pointsTo = pointsTo;
    }

    public override int Size => NumberType.I64.Size;
    public override string Name => $"{_pointsTo.Name}*";

    public static CactusType CreatePointer(CactusType pointsTo, int level) {
        if (pointsTo is ErrorType)
            return ErrorType.ERROR;
        
        PointerType previous = (PointerType)pointsTo.GetPointer();

        for (int i = 0; i < level -1 ; i++) {
            previous = (PointerType)previous.GetPointer();
        }
        return previous;
    }

    public override bool CanImplicitCastInto(CactusType other) {
        //TODO
        return false;
    }

    public override bool Equals(object? obj) {
        if (obj is PointerType other) {
            return PointsTo.Equals(other.PointsTo);
        }

        return false;
    }

    public override int GetHashCode() {
        return _pointsTo.GetHashCode();
    }
}