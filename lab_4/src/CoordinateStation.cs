using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace lab_4;

public class CoordinateStation: AbstractAts
{
    public new static List<AbstractAts> _AllObjects { get; set; } = new List<AbstractAts>(){};
    public override List<AbstractAts> AllObjects { get => _AllObjects; set => _AllObjects = value; }
    
    private static IEnumerable<PropertyInfo> _PublicProperties;
    protected override IEnumerable<PropertyInfo> PublicProperties
    {
        get => CoordinateStation._PublicProperties;
        set => CoordinateStation._PublicProperties = value;
    }
    static CoordinateStation()
    {
        _PublicProperties = typeof(CoordinateStation)
            .GetProperties()
            .Where(x => x.GetMethod != null && x.GetMethod.IsPublic && !x.GetMethod.IsStatic);
    }
    /// <summary>
    /// Количество маркёров (устанавливают соединение на отдельных ступенях искания по информации, получаемой от регистра)
    /// </summary>
    public int? CountOfMarkers { get; set; } = null;

    /// <summary>
    /// Количество регистов (принимают и запоминают информацию)
    /// </summary>
    public int? CountOfRegisters { get; set; } = null;
    
    public CoordinateStation() : base()
    {
        _AllObjects.Add(this);
    }
    
        
    /// <summary> установить значение любого поля по его имени</summary>
    // public override bool SetSomeValue(string targetFieldName, object newFieldValue)
    // {
    //     bool status = false;
    //     PropertyInfo? findField;
    //     if ((findField = PublicProperties.FirstOrDefault(i => i.Name == targetFieldName)) is not null)
    //     {
    //         Type t = Nullable.GetUnderlyingType(findField.PropertyType) ?? findField.PropertyType;
    //         object safeValue = (newFieldValue == null) ? null : Convert.ChangeType(newFieldValue, t);
    //         findField.SetValue(this, safeValue, null);
    //         status = true;
    //     }
    //
    //     return status;
    // }
    //
    // /// <summary> получить значения любого поля из объекта </summary>
    // public override object GetSomeValue(string targetFieldName)
    // {
    //     PropertyInfo? findField;
    //     if ((findField = PublicProperties.FirstOrDefault(i => i.Name == targetFieldName)) is not null)
    //     {
    //         // ((a = x.GetValue(this)) == null ? "<unset>" : $"\"{a.ToString()}\"")}")
    //         return findField.GetValue(this);
    //     }
    //
    //     return null;
    // }
    
}