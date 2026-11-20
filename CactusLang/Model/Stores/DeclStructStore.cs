using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.MemberHolders.Structs;
using CactusLang.Model.Holders;

namespace CactusLang.Model.Stores;

public class DeclStructStore {
    private readonly OrderedDictionary<string, CactusStruct> _fields = new();

    public CactusStruct? GetStruct(string name) => _fields.ContainsKey(name) ? _fields[name] : null;

    public bool AddStruct(CactusStruct cactusStruct) => _fields.TryAdd(cactusStruct.Name, cactusStruct);

    public List<CactusStruct> GetStructs() => _fields.Values.ToList();
}