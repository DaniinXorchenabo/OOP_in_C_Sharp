using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace lab_2;

public class TelephoneStation : IDisposable
{
    private static readonly IEnumerable<PropertyInfo> PublicProperties;
    public static int ObjectCounter { get; private set; } = 0;
    private bool _disposed = false;

    static TelephoneStation()
    {
        PublicProperties = typeof(TelephoneStation)
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

    public TelephoneStation(
        string? address = null,
        int? countOfUsers = null,
        decimal? monthlySubscriptionFee = null,
        string? companyName = null,
        string? inn = null,
        float? trustIndex = null,
        string? dateOfFoundation = null)
    {
        Address = address;
        CountOfUsers = countOfUsers;
        MonthlySubscriptionFee = monthlySubscriptionFee;
        CompanyName = companyName;
        Inn = inn;
        TrustIndex = trustIndex;
        DateOfFoundation = dateOfFoundation;
        ObjectCounter++;
    }

    public TelephoneStation()
    {
        ObjectCounter++;
    }


    public TelephoneStation(string? companyName)
    {
        CompanyName = companyName;
        ObjectCounter++;
    }

    public TelephoneStation(string? companyName, string? inn)
    {
        CompanyName = companyName;
        Inn = inn;
        ObjectCounter++;
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
        if (_disposed) return;
        if (disposing)
        {
            // Освобождаем управляемые ресурсы
        }

        // освобождаем неуправляемые объекты
        _disposed = true;
        ObjectCounter--;
    }

    /// <summary> Деструктор</summary>
    ~TelephoneStation()
    {
        Dispose(false);
    }
}