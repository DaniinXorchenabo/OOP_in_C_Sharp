using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace lab_4;

public class MachineStation: AbstractAts
{
    private static readonly IEnumerable<PropertyInfo> PublicProperties;

    static MachineStation()
    {
        PublicProperties = typeof(MachineStation)
            .GetProperties()
            .Where(x => x.GetMethod != null && x.GetMethod.IsPublic && !x.GetMethod.IsStatic);
    }
    
    /// <summary>
    /// Количество валов
    /// </summary>
    private int? CountOfShaft { get; set; } = null;


}