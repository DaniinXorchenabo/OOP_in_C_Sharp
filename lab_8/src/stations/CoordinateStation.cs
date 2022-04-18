using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace lab_8;
[Serializable]
public class CoordinateStation : AbstractAts
{
    [XmlIgnore] 
    public static Type Serializer { get; set; } = null!;
    
    [XmlIgnore]
    public new static PhoneStationDict<Guid, AbstractAts> _AllObjects { get; set; } = new PhoneStationDict<Guid, AbstractAts>() { };

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
        get => CoordinateStation._PublicProperties;
        set => CoordinateStation._PublicProperties = value;
    }
    [XmlIgnore]
    public new static IEnumerable<PropertyInfo> StaticPublicProperties
    {
        get => CoordinateStation._PublicProperties;
        private set => CoordinateStation._PublicProperties = value;
    }
    static CoordinateStation()
    {
        Serializer = typeof(AbstractAtsCollection<CoordinateStation>);
        _PublicProperties = typeof(CoordinateStation)
            .GetProperties()
            .Where(x => x.GetMethod != null && x.GetMethod.IsPublic && !x.GetMethod.IsStatic);
    }

    /// <summary>
    /// Количество маркёров (устанавливают соединение на отдельных ступенях искания по информации, получаемой от регистра)
    /// </summary>
    [XmlElement("CountOfMarkers")]
    public int? CountOfMarkers { get; set; } = null;

    /// <summary>
    /// Количество регистов (принимают и запоминают информацию)
    /// </summary>
    [XmlElement("CountOfRegisters")]
    public int? CountOfRegisters { get; set; } = null;

    public CoordinateStation() : base()
    {
        _AllObjects += this;
    }
    public CoordinateStation(Random random) : base(random)
    {
        _AllObjects += this;
    }

    public override void CreateCustomizedName()
    {
        CompanyName = "Кастомное имя координатной станции";
    }
}

// public class CoordinateStationCollection
// {
//     [XmlArray("Collection"), XmlArrayItem("Item")]
//     public List<AbstractAts> Collection {get; set;}
// }