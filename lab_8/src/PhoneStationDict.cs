using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace lab_8;

public class PhoneStationDict<TKey, TValue>:  ICollection<KeyValuePair<TKey,TValue>> where TValue: AbstractAts
{
    protected Dictionary<TKey, TValue> Container = new Dictionary<TKey, TValue>();
    public int Count { get; }
    public bool IsReadOnly { get; }

    public void Add(KeyValuePair<TKey, TValue> keyVal)
    {
        Container.Add(keyVal.Key, keyVal.Value);
    }
    
    public void Clear()
    {
        Container.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return Container.Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        if (arrayIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(array));
        }

        if (array.LongLength > arrayIndex + Container.Count)
        {
            throw new ArgumentException();
        }

        foreach(var item in Container.Select((item, ind) => (item: item, ind: ind + arrayIndex)))
        {
            array[item.ind] = item.item;
        }
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return Container.Remove(item.Key);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return Container.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}