using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace lab_5;

public class CoordinateStation : AbstractAts
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
    public CoordinateStation(Random random) : base(random)
    {
        _AllObjects.Add(this);
    }
}