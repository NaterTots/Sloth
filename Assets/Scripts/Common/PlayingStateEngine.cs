using UnityEngine;
using System.Collections;
using System;

public class PlayingStateEngine : IController
{
    public enum States
    {
        NullState,
        StartingLevel,
        Playing,
        Paused,
        LevelEnded
    }

    #region Game State Engine

    private FiniteStateMachine _playingStateMachine = new FiniteStateMachine();

    public States CurrentState
    {
        get
        {
            return _playingStateMachine.GetCurrentState<States>();
        }
    }

    public void ChangePlayingState(States newState)
    {
        States oldState = _playingStateMachine.GetCurrentState<States>();
        if (newState != oldState || newState == States.NullState)
        {
            Debug.Log("Change Playing State : " + newState);

            _playingStateMachine.ChangeState<States>(newState);

            Resolver.Instance.GetController<EventHandler>().Post(Events.PlayingStateTransition.TransitionAway, oldState);
            Resolver.Instance.GetController<EventHandler>().Post(Events.PlayingStateTransition.TransitionTo, newState);
        }
    }

    #endregion Game State Engine

    #region Constructor

    public PlayingStateEngine()
    {
        _playingStateMachine.AddStates<States>(
            States.NullState,
            States.StartingLevel,
            States.Playing,
            States.Paused,
            States.LevelEnded);
    }

    #endregion Constructor

    #region IController
    public void Cleanup()
    {
    }
    #endregion
}
