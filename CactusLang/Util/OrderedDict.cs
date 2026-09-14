using System.Collections.Specialized;

namespace CactusLang.Util;

public class OrderedDict<TKey, TValue> : Dictionary<TKey, TValue> {
    private readonly List<TKey> _orderedKeys;
    
    public OrderedDict(){
        _orderedKeys = new List<TKey>();
    }
    
    public new void Add(TKey key, TValue value) {
        base.Add(key, value);
        _orderedKeys.Add(key);
    }
    public TValue GetByIndex(int index) => this[_orderedKeys[index]];

    public new void Remove(TKey key) {
        int keyIndex = _orderedKeys.IndexOf(key);
        _orderedKeys.RemoveAt(keyIndex);
        base.Remove(key);
    }
}