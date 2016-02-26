using UnityEngine;
using System.Collections;

public class GlobalSettings : MonoBehaviour, IController
{
    //public TerrainConfiguration terrainConfiguration;


    public static string TerrainConfiguration = "GobalSettings.TerrainConfiguration";

    void Awake()
    {
        //Resolver.Instance.GetController<ConfigurationManager>().AddSetting<TerrainConfiguration>(TerrainConfiguration, terrainConfiguration);
    }

    public void Cleanup()
    {

    }
}
