using System.Numerics;

using INT = System.Numerics.BigInteger;


namespace CactusLang.Model.Types.PrimitiveTypes;



public class NumberType : PrimitiveType {
	private static bool _hasBeenInitialized = false;

	private readonly HashSet<NumberType> _subset;
	private readonly string _id;
	public override string Name => _id;
	private int _size;
	public override int Size => _size;


	public override bool IsInteger => Kind is PrimType.Integer or PrimType.Uint;
	public override bool IsNumber => Kind == PrimType.Float || IsInteger;
	public override bool IsUnsigned => Kind == PrimType.Uint;

    
	private NumberType(string id, PrimType kind, int size, long lower = 0x00, ulong upper =0x00) {
		_id = id;
		Kind = kind;
		_size = size;
		_subset = new HashSet<NumberType>();
		Primitives.Add(this);

		LowerBound = lower;
		UpperBound = upper;
	}

	private void Includes(NumberType type) {
		_subset.Add(type);
		_subset.UnionWith(type._subset);
	}

	public override bool CanImplicitCastInto(BaseType superiorType) {
		if (superiorType is NumberType superiorPrim) {
			return (superiorPrim._subset.Contains(this) || Equals(superiorPrim));
		}
		if (superiorType is LiteralIntegerType superiorInt) {
			return superiorInt.CanBeUsedAs(this);
		}
		return false;
	}
    
	public INT LowerBound { get; private set; }
	public INT UpperBound { get; private set; }
    
	public NumberType AsSigned { get; private set; }
	public NumberType AsFloat { get; private set; }

	#region Primitives

	private static readonly HashSet<NumberType> Primitives = new();

	public static HashSet<NumberType> GetPrimitives() => [ ..Primitives ];
    
    
	public static NumberType GetCommonFitType(NumberType A, NumberType B) {
		//If one of them is a float, return type will be a float
		if (A.Kind is PrimType.Float || B.Kind is PrimType.Float) {
			var aFloat = A.AsFloat;
			var bFloat =  B.AsFloat;
			return aFloat.Size> bFloat.Size ? aFloat : bFloat;
		}

		if (A.IsUnsigned || B.IsUnsigned) {
			var aSigned =  A.AsSigned;
			var  bSigned =  B.AsSigned;
			return aSigned.Size > bSigned.Size ? aSigned : bSigned;
		}

		return A.Size > B.Size ? A : B;
	}

	//new HashSet<CctsPrimitive>(primitives);
	public static void InitPrimitives() {
		if (_hasBeenInitialized) return;
		_hasBeenInitialized = true;

		I16.Includes(I08);
		I16.Includes(U08);
		U16.Includes(U08);

		I32.Includes(I16);
		I32.Includes(U16);
		U32.Includes(U16);

		I64.Includes(I32);
		I64.Includes(U32);
		U64.Includes(U32);

		F32.Includes(I16);
		F32.Includes(U16);

		F64.Includes(F32);
		F64.Includes(I32);
		F64.Includes(U32);

		F128.Includes(F64);
		F128.Includes(I64);
		F128.Includes(U64);

		CH16.Includes(CH08);
		CH32.Includes(CH16);


		I08.SetAsNumbers(I08, F32);
		U08.SetAsNumbers(I16, F32);
        
		I16.SetAsNumbers(I16, F32);
		U16.SetAsNumbers(I32, F32);
        
		I32.SetAsNumbers(I32, F32);
		U32.SetAsNumbers(I64, F32);
        
		I64.SetAsNumbers(I64, F64);
		U64.SetAsNumbers(null, F64);
		
		Console.WriteLine("INIT COMPLETE! :)");
	}

	private void SetAsNumbers(NumberType asSigned, NumberType asFloat) {
		AsFloat =  asFloat;
		AsSigned =  asSigned;
	}

	public static readonly NumberType F32 = new("f32", PrimType.Float, 4);
	public static readonly NumberType F64 = new("f64", PrimType.Float, 8);
	public static readonly NumberType F128 = new("f128", PrimType.Float, 16);

	//Integer types
	public static readonly NumberType I08 = new("i08", PrimType.Integer, 1, -128, 127);
	public static readonly NumberType I16 = new("i16", PrimType.Integer, 2, -32768, 32767);
	public static readonly NumberType I32 = new("i32", PrimType.Integer, 4,-2147483648,2147483647);
	public static readonly NumberType I64 = new("i64", PrimType.Integer, 	8,-9223372036854775807);

	public static readonly NumberType U08 = new("u08", PrimType.Uint, 1);
	public static readonly NumberType U16 = new("u16", PrimType.Uint, 2);
	public static readonly NumberType U32 = new("u32", PrimType.Uint, 4);
	public static readonly NumberType U64 = new("u64", PrimType.Uint, 8);

	//Charater types
	public static readonly NumberType CH08 = new("ch08", PrimType.Char, 1);
	public static readonly NumberType CH16 = new("ch16", PrimType.Char, 2);
	public static readonly NumberType CH32 = new("ch32", PrimType.Char, 4);

	public static readonly NumberType BOOL = new("bool", PrimType.Bool, 1);
	public static readonly NumberType VOID = new("void", PrimType.Void, 0);

	#endregion

	public HashSet<NumberType> GetSubset() => _subset;

	//public override string CName => Name;
}