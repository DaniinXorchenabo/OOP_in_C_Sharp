using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace lab_8;


[XmlInclude(typeof(MachineStation))]
[XmlInclude(typeof(CoordinateStation))]
[Serializable]
public abstract class AbstractAts : IDisposable
{
    [XmlIgnore] 
    public static Type Serializer { get; set; } = null!;
    
    [XmlIgnore]
    private static IEnumerable<PropertyInfo> _PublicProperties;

    [XmlIgnore]
    protected virtual IEnumerable<PropertyInfo> PublicProperties
    {
        get => AbstractAts._PublicProperties;
        set => AbstractAts._PublicProperties = value;
    }

    [XmlIgnore]
    public static int ObjectCounter { get; private set; } = 0;
    [XmlIgnore]
    protected bool Disposed = false;
    [XmlIgnore]
    private static PhoneStationDict<Guid, AbstractAts> _AllTelephoneStations { get; set; } = new PhoneStationDict<Guid, AbstractAts> { };

    public static PhoneStationDict<Guid, AbstractAts> AllTelephoneStations
    {
        get => _AllTelephoneStations;
    }
    
    [XmlIgnore]
    public static PhoneStationDict<Guid, AbstractAts>  _AllObjects { get; set; } = new PhoneStationDict<Guid, AbstractAts>  { };
    [XmlIgnore]
    protected virtual PhoneStationDict<Guid, AbstractAts>  AllObjects
    {
        get => _AllObjects;
        set => _AllObjects = value;
    }

    public static event Action<AbstractAts> AddItemEvent;
    public static event Action<AbstractAts> RemoveItemEvent;

    static AbstractAts()
    {
        Serializer = typeof(AbstractAtsCollection<AbstractAts>);
        _PublicProperties = typeof(AbstractAts)
            .GetProperties()
            .Where(x => x.GetMethod != null && x.GetMethod.IsPublic && !x.GetMethod.IsStatic);
    }
    
    /// <summary> Id компании</summary>
    [XmlElement("Id")]
    public readonly Guid Id = Guid.NewGuid();

    /// <summary> Адресс компании</summary>
    [XmlElement("Address")]
    public string? Address { get; set; } = null;

    /// <summary> Количество клиентов компании</summary>
    [XmlElement("CountOfUsers")]
    public int? CountOfUsers { get; set; } = null;

    /// <summary> стоимость ежемесячных услуг</summary>
    [XmlElement("MonthlySubscriptionFee")]
    public Decimal? MonthlySubscriptionFee { get; set; } = null;

    /// <summary> Название компании</summary>
    [XmlElement("CompanyName")]
    public string? CompanyName { get; set; } = null;

    /// <summary> ИНН компании</summary>
    [XmlElement("Inn")]
    public string? Inn { get; set; } = null;

    /// <summary> Индекс доверия пользователей этой компании</summary>
    [XmlElement("TrustIndex")]
    public float? TrustIndex { get; set; } = null;

    /// <summary> Дата основания компании</summary>
    [XmlElement("DateOfFoundation")]
    public string? DateOfFoundation { get; set; } = null;

    public AbstractAts()
    {
        ObjectCounter++;
        AbstractAts._AllTelephoneStations += this;
    }

    public AbstractAts(Random random)
    {

       
        foreach (var field in PublicProperties)
        {
            if (field.PropertyType == typeof(string))
            {
                SetSomeValue(field.Name, GenerateChar(
                    Convert.ToInt32(Math.Floor(20 * random.NextDouble() + 1)),
                    random,
                    65,
                    122
                ));
            }
            else if (field.PropertyType == typeof(int) || field.PropertyType == typeof(int?))
            {
                SetSomeValue(field.Name, Convert.ToInt32(Math.Floor(100000 * random.NextDouble())));
            }
            else if (field.PropertyType == typeof(float) || field.PropertyType == typeof(float?))
            {
                SetSomeValue(field.Name, ((100000 * random.NextDouble())));
            }
            else if (field.PropertyType == typeof(double) || field.PropertyType == typeof(double?))
            {
                SetSomeValue(field.Name, Convert.ToDouble((100000 * random.NextDouble())));
            }
            else if (field.PropertyType == typeof(Decimal) || field.PropertyType == typeof(Decimal?))
            {
                SetSomeValue(field.Name, Convert.ToDecimal((1000 * random.NextDouble())));
            }
        }
        AbstractAts._AllTelephoneStations += this;
        ObjectCounter++;
    }

    public string GenerateChar(Random random, int minInterval = 48, int maxInterval = 58)
    {
        return Convert
            .ToChar(Convert.ToInt32(Math.Floor((maxInterval - minInterval) * random.NextDouble() + minInterval)))
            .ToString();
    }

    public string GenerateChar(int count, Random random, int minInterval = 48, int maxInterval = 58)
    {
        string randomString = "";

        for (int i = 0; i < count; i++)
        {
            randomString += GenerateChar(random, minInterval, maxInterval);
        }

        return randomString;
    }

    /// <summary> переопределение для печати объекта</summary>
    public override string ToString()
    {
        return $"{CompanyName ?? "<unknown>"}";
    }

    /// <summary> Получить полный</summary>
    public string ToLongString()
    {
        return $"ATC=>[{string.Join(", ", ParamsAsStrings())}]";
    }


    /// <summary> Получить пары "название параметра - значение" в виде строки</summary>
    public IEnumerable<string> ParamsAsStrings()
    {
        object? a;
        return PublicProperties.Select(
            x => $"{x.Name}={((a = x.GetValue(this)) == null ? "<unset>" : $"\"{a.ToString()}\"")}");
    }

    /// <summary> Напечатать название компании</summary>
    public void GetCompanyName()
    {
        Console.WriteLine($"А название этой компании - {CompanyName ?? "<Сокрыто мировым правительством рептилий>"}");
    }

    /// <summary> получить все поля объекта</summary>
    public List<string> GetAllFields()
    {
        return PublicProperties.Select(x => x.Name).ToList();
    }

    /// <summary> установить значение любого поля по его имени</summary>
    public bool SetSomeValue(string targetFieldName, object newFieldValue)
    {
        bool status = false;
        PropertyInfo? findField;
        if ((findField = PublicProperties.FirstOrDefault(i => i.Name == targetFieldName)) is not null)
        {
            Type t = Nullable.GetUnderlyingType(findField.PropertyType) ?? findField.PropertyType;
            object safeValue = (newFieldValue == null) ? null : Convert.ChangeType(newFieldValue, t);
            findField.SetValue(this, safeValue, null);
            status = true;
        }

        return status;
    }

    /// <summary> получить значения любого поля из объекта </summary>
    public object GetSomeValue(string targetFieldName)
    {
        PropertyInfo? findField;
        if ((findField = PublicProperties.FirstOrDefault(i => i.Name == targetFieldName)) is not null)
        {
            // ((a = x.GetValue(this)) == null ? "<unset>" : $"\"{a.ToString()}\"")}")
            return findField.GetValue(this);
        }

        return null;
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
        if (Disposed) return;
        if (disposing)
        {
            // Освобождаем управляемые ресурсы
        }

        // освобождаем неуправляемые объекты
        Disposed = true;
        ObjectCounter--;
        _AllTelephoneStations -= this;
        AllObjects -= this;
    }

    /// <summary> Деструктор</summary>
    ~AbstractAts()
    {
        Dispose(false);
    }

    public static PhoneStationDict<Guid, AbstractAts> operator +(PhoneStationDict<Guid, AbstractAts>  list, AbstractAts station)
    {
        list.Add( new KeyValuePair<Guid, AbstractAts>(station.Id, station));
        if (list == _AllTelephoneStations)
        {
            AddItemEvent(station);
        }
        return list;
    }
    public static PhoneStationDict<Guid, AbstractAts> operator -(PhoneStationDict<Guid, AbstractAts>  list, AbstractAts station)
    {
        list.Remove(new KeyValuePair<Guid, AbstractAts>(station.Id, station));
        if (list == _AllTelephoneStations)
        {
            RemoveItemEvent(station);
        }
        return list;
    }

    public abstract void CreateCustomizedName();

}

public class AbstractAtsCollection<TAtsType> where TAtsType: AbstractAts
{
    [XmlArray("Collection"), XmlArrayItem("Item")]
    public List<AbstractAts> Collection {get; set;}

    public AbstractAtsCollection(List<AbstractAts> collection)
    {
        Collection = collection;
    }

    public AbstractAtsCollection()
    {
        
    }
    
}