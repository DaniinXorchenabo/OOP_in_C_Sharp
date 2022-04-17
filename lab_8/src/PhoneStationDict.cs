using System.Collections;
using System.Collections.Generic;

namespace lab_8;

public class PhoneStationDict<TKey, TValue>:  ICollection<KeyValuePair<TKey,TValue>>
{
    protected Dictionary<TKey, TValue> Container = new Dictionary<TKey, TValue>();
    public int Count { get; }
    public bool IsReadOnly { get; }

    public void Add(KeyValuePair<TKey, TValue> f)
    {
        throw new System.NotImplementedException();
    }
    
    public void Clear()
    {
        throw new System.NotImplementedException();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        throw new System.NotImplementedException();
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        throw new System.NotImplementedException();
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}