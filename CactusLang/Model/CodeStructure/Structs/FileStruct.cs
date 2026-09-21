using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Structs;

public class FileStruct {
    public string Name { get; }

    public CodeFile CodeFile { get; }

    public FieldStore<StructField> Fields { get; }

    public FunctionStore<StructFunction> Functions { get; }

    public Type StructType { get; }

    public FileStruct(string id,  CodeFile codeFile) {
        Name = id;
        CodeFile = codeFile;
        Fields = new FieldStore<StructField>();
        Functions = new FunctionStore<StructFunction>();
        StructType = new(this);
    }


    
    
    
    public class Type : BaseType {
        private readonly FileStruct _fileStruct;
        public FileStruct FileStruct => _fileStruct;
        public override string Name => _fileStruct.Name;

        internal Type(FileStruct fileStruct) {
            _fileStruct = fileStruct;
        }

        public override int Size => -999; //TODO C generálás alapján kiszámolni...

        protected override bool CanImplicitCastInto(BaseType other) {
            //TODO
            return false;
        }
    }
}