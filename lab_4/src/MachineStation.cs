using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace lab_4;

public class MachineStation: AbstractAts
{
    public static List<AbstractAts> AllObjects { get; set; } = new List<AbstractAts>(){};
    private static IEnumerable<PropertyInfo> _PublicProperties;
    protected override IEnumerable<PropertyInfo> PublicProperties
    {
        get => MachineStation._PublicProperties;
        set => MachineStation._PublicProperties = value;
    }

    static MachineStation()
    {
        _PublicProperties = typeof(MachineStation)
            .GetProperties()
            .Where(x => x.GetMethod != null && x.GetMethod.IsPublic && !x.GetMethod.IsStatic);
    }
    
    /// <summary>
    /// Количество валов
    /// </summary>
    public int? CountOfShaft { get; set; } = null;

    public MachineStation() : base()
    {
        AllObjects.Add(this);
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