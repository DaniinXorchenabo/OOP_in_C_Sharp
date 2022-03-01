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

    public string? Address { get; set; } = null;
    public int? CountOfUsers { get; set; } = null;
    public Decimal? MonthlySubscriptionFee { get; set; } = null;    
    public string? CompanyName { get; set; } = null;
    public string? Inn { get; set; } = null;
    public float? TrustIndex { get; set; } = null;
    public string? DateOfFoundation { get; set; } = null;

    public TelephoneStation (
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

    public TelephoneStation(){}


    public TelephoneStation(string? companyName)
    {
        CompanyName = companyName;
    }

    public TelephoneStation(string? companyName, string? inn)
    {
        CompanyName = companyName;
        Inn = inn;
    }

    public override string ToString()
    {
        object? currentValue;
        var paramsAsStr = PublicProperties
            .Select(x => 
                $"{x.Name}={((currentValue = x.GetValue(this)) == null ? "<unset>" : $"\"{currentValue}\"")}");
        return $"ATC=>[{string.Join(", ", paramsAsStr)}]";
    }

    public void GetCompanyName()
    {
        Console.WriteLine($"А название этой компании - {CompanyName ?? "<Сокрыто мировым правительством рептилий>"}");
    }

    public List<string> GetAllFields()
    {
        return PublicProperties.Select(x => x.Name).ToList();
    }

    public bool SetSomeValue(string targetFieldName, string newFieldValue)
    {
        PropertyInfo ? findField;
        if ((findField = PublicProperties.FirstOrDefault(i => i.Name == targetFieldName)) is not null)
        {
            // var _type = findField.PropertyType;
            findField.SetValue(this, newFieldValue);
            return true;
        }

        return false;
    }
}