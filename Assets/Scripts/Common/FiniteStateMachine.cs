using System;
using System.Collections;

public class FiniteStateMachine
{
    public delegate void StateExec();

    private struct StateCallback
    {
        internal int stateNum;
        internal StateExec callback;
    }

    StateCallback[] stateCallbacks;

    private StateCallback currentCallback;

    #region Initialization

    public void AddStates(params int[] states)
    {
        if (stateCallbacks != null) throw new ArgumentException("Can't add states once AddStates has already been called");

        stateCallbacks = new StateCallback[states.Length];

        for (int i = 0; i < states.Length; i++)
        {
            stateCallbacks[i].stateNum = states[i];
        }
    }

    public void AddStates<T>(params T[] states) where T : struct, IConvertible
    {
        if (stateCallbacks != null) throw new ArgumentException("Can't add states once AddStates has already been called");

        stateCallbacks = new StateCallback[states.Length];

        for (int i = 0; i < states.Length; i++)
        {
            stateCallbacks[i].stateNum = Convert.ToInt32(states[i]);
        }
    }

    public void AddCallbacks(params StateExec[] callbacks)
    {
        if (callbacks.Length != stateCallbacks.Length)
            throw new FormatException("When using AddCallbacks, must provide the same amount of parameters as a subsequent AddStates call.");

        for (int i = 0; i < callbacks.Length; i++)
        {
            stateCallbacks[i].callback = callbacks[i];
        }
    }

    public void AddStateCallback(int state, StateExec callback)
    {
        for (int i = 0; i < stateCallbacks.Length; i++)
        {
            if (stateCallbacks[i].stateNum == state)
            {
                stateCallbacks[i].callback = callback;
                break;
            }
        }
    }

    #endregion Initialization

    #region Getters

    public int CurrentState
    {
        get
        {
            return currentCallback.stateNum;
        }
    }

    public T GetCurrentState<T>() where T : struct, IConvertible
    {
        return (T)Enum.ToObject(typeof(T), currentCallback.stateNum);
    }

    #endregion Getters

    #region Changing and Executing States

    public bool ChangeState(int state)
    {
        bool bStateExists = false;

        foreach (StateCallback stateCallback in stateCallbacks)
        {
            if (stateCallback.stateNum == state)
            {
                currentCallback = stateCallback;
                bStateExists = true;
                break;
            }
        }

        return bStateExists;
    }

    public bool ChangeState<T>(T state) where T : struct, IConvertible
    {
        return ChangeState(Convert.ToInt32(state));
    }

    public void Execute()
    {
        currentCallback.callback();
    }

    #endregion Changing and Executing States
}
