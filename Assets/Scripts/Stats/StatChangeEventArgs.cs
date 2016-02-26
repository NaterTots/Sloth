using UnityEngine;
using System.Collections;

/// <summary>
/// TODO: not a huge fan of this.  Especially that it's not being called from within stats management itself.  I think in the future I'll look at 
/// blending this into stats and having listeners available within the stats management.
/// </summary>
public class StatChangeEventArgs
{
    public string StatName { get; private set; }

    public StatChangeEventArgs(string name)
    {
        StatName = name;
    }
}
