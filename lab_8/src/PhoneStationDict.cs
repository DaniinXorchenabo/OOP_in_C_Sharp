using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace lab_8;

public class PhoneStationDict<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>> where TValue : AbstractAts, IDisposable
{
    protected Dictionary<TKey, TValue> Container = new Dictionary<TKey, TValue>();
    private bool _disposed = false;
    public int Count
    {
        get => Container.Count;
    }

    public bool IsReadOnly { get; }

    public PhoneStationDict()
    {
        Container = new Dictionary<TKey, TValue>();
    }

    public PhoneStationDict(string keyParamName, params TValue[] data)
    {
        Container = new Dictionary<TKey, TValue>();
        var getter = typeof(TValue)
            .GetProperty(keyParamName);
        var keyValuePairs = data.Select(x =>
        {
            var typedKey = getter != null && getter.GetValue(x) is TKey ? (TKey) getter.GetValue(x) : default;
            var t = new KeyValuePair<TKey, TValue>(typedKey, x);
            this.Add(t);
            return t;
        });
    }

    public TValue this[TKey index]
    {
        get => Container[index];
        set => Container[index] = value;
    }

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

        foreach (var item in Container.Select((item, ind) => (item: item, ind: ind + arrayIndex)))
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
    
    /// <summary> Деструктор</summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary> Деструктор</summary>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            // Освобождаем управляемые ресурсы
        }

        // освобождаем неуправляемые объекты
        _disposed = true;
    }

    /// <summary> Деструктор</summary>
    ~PhoneStationDict()
    {
        Dispose(false);
    }
}