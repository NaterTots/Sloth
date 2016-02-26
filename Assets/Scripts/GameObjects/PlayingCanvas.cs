using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayingCanvas : MonoBehaviour
{
    Text score;
    StatsManager statsManager;

	// Use this for initialization
	void Start ()
    {
        score = transform.FindChild("Score").gameObject.GetComponent<Text>();
        score.text = "0";

        Resolver.Instance.GetController<EventHandler>().Register(Events.Framework.StatChanged, StatChanged);
        statsManager = Resolver.Instance.GetController<StatsManager>();
	}
	
    void OnDestroy()
    {
        Resolver.Instance.GetController<EventHandler>().UnRegister(Events.Framework.StatChanged, StatChanged);
    }

	void StatChanged (int id, object data)
    {
	    if (((StatChangeEventArgs)data).StatName.Equals(GlobalStats.GameStats))
        {
            score.text = ((GameStats)statsManager.GetSettingValue(GlobalStats.GameStats)).score.ToString();
        }
	}
}
