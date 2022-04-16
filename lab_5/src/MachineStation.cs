using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace lab_5;

public class MachineStation : AbstractAts
{
    public new static List<AbstractAts> _AllObjects { get; set; } = new List<AbstractAts>() { };

    protected override List<AbstractAts> AllObjects
    {
        get => _AllObjects;
        set => _AllObjects = value;
    }

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
        _AllObjects += this;
    }
    public MachineStation(Random random) : base(random)
    {
        _AllObjects += this;
    }
    public override void CreateCustomizedName()
    {
        CompanyName = "Кастомное имя механической станции";
    }
}