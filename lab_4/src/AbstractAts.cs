using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace lab_4;

public abstract class AbstractAts : IDisposable
{
    private static IEnumerable<PropertyInfo> _PublicProperties;
    protected virtual IEnumerable<PropertyInfo> PublicProperties
    {
        get => AbstractAts._PublicProperties;
        set => AbstractAts._PublicProperties = value;
    }

    public static int ObjectCounter { get; private set; } = 0;
    protected bool Disposed = false;
    private static List<AbstractAts> _AllTelephoneStations { get; set; } = new List<AbstractAts>(){};
    public static List<AbstractAts> _AllObjects { get; set; } = new List<AbstractAts>(){};
    protected virtual List<AbstractAts> AllObjects { get => _AllObjects; set => _AllObjects = value; }

    static AbstractAts()
    {
        _PublicProperties = typeof(AbstractAts)
            .GetProperties()
            .Where(x => x.GetMethod != null && x.GetMethod.IsPublic && !x.GetMethod.IsStatic);
    }

    /// <summary> Адресс компании</summary>
    public string? Address { get; set; } = null;

    /// <summary> Количество клиентов компании</summary>
    public int? CountOfUsers { get; set; } = null;

    /// <summary> стоимость ежемесячных услуг</summary>
    public Decimal? MonthlySubscriptionFee { get; set; } = null;

    /// <summary> Название компании</summary>
    public string? CompanyName { get; set; } = null;

    /// <summary> ИНН компании</summary>
    public string? Inn { get; set; } = null;

    /// <summary> Индекс доверия пользователей этой компании</summary>
    public float? TrustIndex { get; set; } = null;

    /// <summary> Дата основания компании</summary>
    public string? DateOfFoundation { get; set; } = null;

    public AbstractAts()
    {
        ObjectCounter++;
        AbstractAts._AllTelephoneStations.Add(this);
        
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
        _AllTelephoneStations.Remove(this);
        AllObjects.Remove(this);
    }

    /// <summary> Деструктор</summary>
    ~AbstractAts()
    {
        Dispose(false);
    }
}