using System.Reflection;

namespace lab_1;

public class TelephoneStation
{
    private static readonly IEnumerable<PropertyInfo> PublicProperties;

    static TelephoneStation()
    {
        PublicProperties = typeof(TelephoneStation)
            .GetProperties()
            .Where(x => x.GetMethod != null && x.GetMethod.IsPublic);
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
    }

    public TelephoneStation()
    {
    }


    public TelephoneStation(string? companyName)
    {
        CompanyName = companyName;
    }

    public TelephoneStation(string? companyName, string? inn)
    {
        CompanyName = companyName;
        Inn = inn;
    }

    /// <summary> переопределение для печати объекта</summary>
    public override string ToString()
    {
        object? currentValue;
        var paramsAsStr = PublicProperties
            .Select(x =>
                $"{x.Name}={((currentValue = x.GetValue(this)) == null ? "<unset>" : $"\"{currentValue}\"")}");
        return $"ATC=>[{string.Join(", ", paramsAsStr)}]";
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
    public bool SetSomeValue(string targetFieldName, string newFieldValue)
    {
        PropertyInfo? findField;
        if ((findField = PublicProperties.FirstOrDefault(i => i.Name == targetFieldName)) != null)
        {
            Type t = Nullable.GetUnderlyingType(findField.PropertyType) ?? findField.PropertyType;
            object safeValue = (newFieldValue == null) ? null : Convert.ChangeType(newFieldValue, t);
            findField.SetValue(this, safeValue, null);
            return true;
        }

        return false;
    }
}