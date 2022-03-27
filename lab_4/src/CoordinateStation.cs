using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace lab_4;

public class CoordinateStation: AbstractAts
{
    public new static List<AbstractAts> AllObjects { get; set; } = new List<AbstractAts>(){};

    static CoordinateStation()
    {
        PublicProperties = typeof(CoordinateStation)
            .GetProperties()
            .Where(x => x.GetMethod != null && x.GetMethod.IsPublic && !x.GetMethod.IsStatic);
    }
    /// <summary>
    /// Количество маркёров (устанавливают соединение на отдельных ступенях искания по информации, получаемой от регистра)
    /// </summary>
    private int? CountOfMarkers { get; set; } = null;

    /// <summary>
    /// Количество регистов (принимают и запоминают информацию)
    /// </summary>
    private int? CountOfRegisters { get; set; } = null;
    
    public CoordinateStation() : base()
    {
        AllObjects.Add(this);
    }
    
}