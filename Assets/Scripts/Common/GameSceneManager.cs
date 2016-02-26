using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

class GameSceneManager : MonoBehaviour, IController
{
    void Awake()
    {
        Resolver.Instance.GetController<EventHandler>().Register(Events.GameStateTransition.TransitionTo, OnTransitionToState);

        //no matter what scene we start in, we want to begin with the null state
        Resolver.Instance.GetController<GameStateEngine>().ChangeGameState(GameStateEngine.States.NullState);
    }

    void OnTransitionToState(int id, object param)
    {
        GameStateEngine.States newState = (GameStateEngine.States)param;

        switch (newState)
        {
            case GameStateEngine.States.NullState:
                SceneManager.LoadScene("EmptyStartScene");
                break;
            case GameStateEngine.States.Title:
                SceneManager.LoadScene("Title");
                break;
            case GameStateEngine.States.Playing:
                switch ((GameModeConfiguration.GameMode)Resolver.Instance.GetController<StatsManager>().GetSettingValue(GlobalStats.GameMode))
                {
                    case GameModeConfiguration.GameMode.OneSloth:
                        SceneManager.LoadScene("OneSloth");
                        break;
                    case GameModeConfiguration.GameMode.TwoSloths:
                        SceneManager.LoadScene("TwoSloth");
                        break;
                }
                break;
            case GameStateEngine.States.GameOver:
                SceneManager.LoadScene("MainMenu");
                break;
            case GameStateEngine.States.Credits:
                SceneManager.LoadScene("Credits");
                break;
            case GameStateEngine.States.Settings:
                SceneManager.LoadScene("Settings");
                break;
            default:
                Debug.LogError("Invalid state transition");
                break;
        }
    }

    #region IController

    public void Cleanup()
    {

    }

    #endregion IController


}