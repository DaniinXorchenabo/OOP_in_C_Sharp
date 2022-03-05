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

    public string? Address { get; set; } = null;
    public int? CountOfUsers { get; set; } = null;
    public Decimal? MonthlySubscriptionFee { get; set; } = null;
    public string? CompanyName { get; set; } = null;
    public string? Inn { get; set; } = null;
    public float? TrustIndex { get; set; } = null;
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

    public override string ToString()
    {
        return $"{CompanyName ?? "<unknown>"}";
    }

    public string ToLongString()
    {
        return $"ATC=>[{string.Join(", ", ParamsAsStrings())}]";
    }

    public IEnumerable<string> ParamsAsStrings()
    {
        object? a;
        return PublicProperties.Select(
            x => $"{x.Name}={((a = x.GetValue(this)) == null ? "<unset>" : $"\"{a.ToString()}\"")}");
    }

    public void GetCompanyName()
    {
        Console.WriteLine($"А название этой компании - {CompanyName ?? "<Сокрыто мировым правительством рептилий>"}");
    }

    public List<string> GetAllFields()
    {
        return PublicProperties.Select(x => x.Name).ToList();
    }

    public bool SetSomeValue(string targetFieldName, object newFieldValue)
    {
        PropertyInfo? findField;
        if ((findField = PublicProperties.FirstOrDefault(i => i.Name == targetFieldName)) is not null)
        {
            try
            {
                Type t = Nullable.GetUnderlyingType(findField.PropertyType) ?? findField.PropertyType;
                object safeValue = (newFieldValue == null) ? null : Convert.ChangeType(newFieldValue, t);
                findField.SetValue(this, safeValue, null);
            } catch (FormatException e)
            {
                return false;
            }

            return true;
        }

        return false;
    }
    
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

    public void Dispose()
    {
        
        Dispose(true);
        GC.SuppressFinalize(this);
        
    }
    
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
 
    // Деструктор
    ~TelephoneStation()
    {
        Dispose (false);
    }
}