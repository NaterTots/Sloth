using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class Resolver
{
    #region Static Instance
    private static object _instanceLock = new object();
    private static Resolver _instance;

    private Resolver()
    {
        AddController(new EventHandler());
        AddController(new ConfigurationManager());
        AddController(new InputController());
        AddController(new StatsManager());
    }

    public static Resolver Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new Resolver();
                    }
                }
            }
            return _instance;
        }
    }
    #endregion

    private Dictionary<System.Type, IController> _controllers = new Dictionary<System.Type, IController>();

    public void AddController(IController c)
    {
        if (c == null)
        {
            Debug.LogError("Controller parameter is null, that means it doesn't derive from IController!");
        }
        // Only allow one object of each type
        if (!ContainsController(c.GetType()))
        {
            _controllers.Add(c.GetType(), c);
        }
    }

    public void RemoveController(IController c)
    {
        if (c == null)
        {
            Debug.LogError("Controller parameter is null, that means it doesn't derive from IController!");
        }

        if (ContainsController(c.GetType()))
        {
            _controllers.Remove(c.GetType());
        }
    }

    /// <summary>
    /// Adds the controller regardless of whether one of the
    /// same type already exists.  If one does already exist, it 
    /// will be removed before the new one is added.
    /// </summary>
    /// <param name="c"></param>
    public void AddOrReplaceController<T>(T controller) where T : IController
    {
        if (ContainsController<T>())
        {
            _controllers.Remove(typeof(T));
        }
        AddController(controller);
    }

    public bool ContainsController<T>() where T : IController
    {
        return _controllers.ContainsKey(typeof(T));
    }

    public bool ContainsController(System.Type t)
    {
        return _controllers.ContainsKey(t);
    }

    public T GetController<T>() where T : class, IController
    {
        if (!ContainsController<T>())
        {
            Debug.LogError("Unable to find controller: " + typeof(T));
        }
        return (T)_controllers[typeof(T)];
    }
}