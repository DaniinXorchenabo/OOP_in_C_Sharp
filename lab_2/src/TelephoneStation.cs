using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace lab_2;

public class TelephoneStation
{
    private static readonly IEnumerable<PropertyInfo> _publicProperties;

    static TelephoneStation()
    {
        _publicProperties = typeof(TelephoneStation)
            .GetProperties()
            .Where(x => x.GetMethod != null && x.GetMethod.IsPublic);
    }

    public string? Address { get; set; } = null;
    public int? CountOfUsers { get; set; } = null;
    public Decimal? MonthlySubscriptionFee { get; set; } = null;
    public string? CompanyName { get; set; } = null;
    public string? INN { get; set; } = null;
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
        INN = inn;
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
        INN = inn;
    }

    public override string ToString()
    {
        object? _a;
        return
            $"ATC=>[{string.Join(", ", _publicProperties.Select(x => $"{x.Name}={((_a = x.GetValue(this)) is null ? "<unset>" : $"\"{_a.ToString()}\"")}"))}]";
    }

    public void GetCompanyName()
    {
        Console.WriteLine($"А название этой компании - {CompanyName ?? "<Сокрыто мировым правительством рептилий>"}");
    }

    public List<string> GetAllFields()
    {
        return _publicProperties.Select(x => x.Name).ToList();
    }

    public bool SetSomeValue(string targetFieldName, string newFieldValue)
    {
        PropertyInfo ? findField;
        if ((findField = _publicProperties.FirstOrDefault(i => i.Name == targetFieldName)) is not null)
        {
            // var _type = findField.PropertyType;
            findField.SetValue(this, newFieldValue);
            return true;
        }

        return false;
    }
}