from enum import Enum
import math

class PrimType(Enum): 
    Signed = 0,
    Uint = 1,
    Float = 2,
    Char = 3,
    Bool = 4,
    Void = 5

INT_TYPES: list[IntType] = []
class IntType:
    min: int
    max: int
    size: int
    as_code: str
    type: PrimType
    compatible_types: list[IntType]
    
    def __init__(self, as_code: str, type: PrimType, size: int):
        self.as_code = as_code
        self.size = size
        self.type = type
        if type == PrimType.Signed:
            # [-2^(n-1) : 2^(n-1)-1]
            self.min = -(2**(size-1))
            self.max = 2**(size-1)-1
            
        elif type == PrimType.Uint:
            # [-2^(n-1) : 2^(n-1)-1]
            self.min = 0
            self.max = 2**(size)-1
        else:
            raise Exception
        
        INT_TYPES.append(self)
        self.compatible_types.append(self)
        
    def includes(self, type: IntType):
        self.compatible_types.append(type)
        

        
#Integer types
I08 = IntType("i08", PrimType.Signed, 1)
I16 = IntType("i16", PrimType.Signed, 2)
I32 = IntType("i32", PrimType.Signed, 4)
I64 = IntType("i64", PrimType.Signed, 8)
    
U08 = IntType("u08", PrimType.Uint, 1)
U16 = IntType("u16", PrimType.Uint, 2)
U32 = IntType("u32", PrimType.Uint, 4)
U64 = IntType("u64", PrimType.Uint, 8)


def init():
    I16.includes(I08)
    I16.includes(U08)
    U16.includes(U08)

    I32.includes(I16)
    I32.includes(U16)
    U32.includes(U16)

    I64.includes(I32)
    I64.includes(U32)
    U64.includes(U32)

#F32 = new("f32", Type.Float, 4)
#F64 = new("f64", Type.Float, 8)
#F128 = new("f128", Type.Float, 16)