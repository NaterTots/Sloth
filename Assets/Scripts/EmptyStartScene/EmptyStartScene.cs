using UnityEngine;
using System.Collections;

public class EmptyStartScene : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
        Resolver.Instance.GetController<GameStateEngine>().ChangeGameState(GameStateEngine.States.Title);
	}
}
