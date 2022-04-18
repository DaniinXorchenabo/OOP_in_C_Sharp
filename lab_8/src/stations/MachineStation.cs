using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace lab_8;
[Serializable]
public class MachineStation : AbstractAts
{
    [XmlIgnore] 
    public static Type Serializer { get; set; } = null!;
    [XmlIgnore]
    public new static PhoneStationDict<Guid, AbstractAts> _AllObjects { get; set; } = new PhoneStationDict<Guid, AbstractAts>{ };

    [XmlIgnore]
    protected override PhoneStationDict<Guid, AbstractAts> AllObjects
    {
        get => _AllObjects;
        set => _AllObjects = value;
    }

    [XmlIgnore]
    private static IEnumerable<PropertyInfo> _PublicProperties;

    [XmlIgnore]
    protected override IEnumerable<PropertyInfo> PublicProperties
    {
        get => MachineStation._PublicProperties;
         set => MachineStation._PublicProperties = value;
    }
    [XmlIgnore]
    public new static IEnumerable<PropertyInfo> StaticPublicProperties
    {
        get => MachineStation._PublicProperties;
        private set => MachineStation._PublicProperties = value;
    }

    static MachineStation()
    {
        Serializer = typeof(AbstractAtsCollection<MachineStation>);
        _PublicProperties = typeof(MachineStation)
            .GetProperties()
            .Where(x => x.GetMethod != null && x.GetMethod.IsPublic && !x.GetMethod.IsStatic);
    }

    /// <summary>
    /// Количество валов
    /// </summary>
    [XmlElement("CountOfShaft")]
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