using UnityEngine;
using System.Collections;

public class EventHandlerExecuter : MonoBehaviour , IController
{

    private EventHandler eventHandler = Resolver.Instance.GetController<EventHandler>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        eventHandler.Execute();
	}

    public void Cleanup()
    {

    }
}
