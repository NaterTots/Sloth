using UnityEngine;
using System.Collections;
using System;

public class GameStateEngine : MonoBehaviour , IController
{
    public enum States
    {
        NullState,
        Title,
        Playing,
        GameOver,
        Settings,
        Credits
    }

    #region Game State Engine

    private FiniteStateMachine _gameStateMachine = new FiniteStateMachine();

    public States CurrentState
    {
        get
        {
            return _gameStateMachine.GetCurrentState<States>();
        }
    }

    public void ChangeGameState(States newState)
    {
        States oldState = _gameStateMachine.GetCurrentState<States>();
        if (newState != oldState || newState == States.Playing)
        {
            Debug.Log("ChangeGameState: " + newState);

            _gameStateMachine.ChangeState<States>(newState);

            Resolver.Instance.GetController<EventHandler>().Post(Events.GameStateTransition.TransitionAway, oldState);
            Resolver.Instance.GetController<EventHandler>().Post(Events.GameStateTransition.TransitionTo, newState);
        }
        else if (newState == States.NullState)
        {
            //if requesting to go to the NullState, drop everything and go there
            Resolver.Instance.GetController<EventHandler>().PostExec(Events.GameStateTransition.TransitionAway, oldState);
            Resolver.Instance.GetController<EventHandler>().PostExec(Events.GameStateTransition.TransitionTo, newState);
        }
    }

    #endregion Game State Engine

    #region MonoBehaviour

    void Awake()
    {
        _gameStateMachine.AddStates<States>(
            States.NullState,
            States.Title,
            States.Playing,
            States.GameOver,
            States.Settings,
            States.Credits);
    }

    #endregion MonoBehaviour

    #region IController
    public void Cleanup()
    {
    }
    #endregion
}
