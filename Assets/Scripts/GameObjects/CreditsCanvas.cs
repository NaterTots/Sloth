using UnityEngine;
using System.Collections;

public class CreditsCanvas : MonoBehaviour
{
    public void OnBack()
    {
        Resolver.Instance.GetController<GameStateEngine>().ChangeGameState(GameStateEngine.States.Title);
    }
}
