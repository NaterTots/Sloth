using System;
using System.Collections.Generic;

class EventHandler : IController
{
    private Dictionary<int, List<EventCallback>> callbacks = new Dictionary<int, List<EventCallback>>();

    private Queue<PendingEvent> pendingEvents = new Queue<PendingEvent>();
    private System.Diagnostics.Stopwatch executionStopwatch = new System.Diagnostics.Stopwatch();

    internal struct PendingEvent
    {
        internal EventCallback Callback;
        internal int EventId;
        internal object Data;
    }

    #region Public Interface

    public delegate void EventCallback(int id, object data);

    /// <summary>
    /// Register a callback for an event, based on the event id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="callback"></param>
    public void Register(int id, EventCallback callback)
    {
        if (callbacks.ContainsKey(id))
        {
            callbacks[id].Add(callback);
        }
        else
        {
            callbacks.Add(
                id,
                new List<EventCallback>() { callback }
                );
        }
    }

    /// <summary>
    /// Unregister a callback for an event, based on the event id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="callback"></param>
    public void UnRegister(int id, EventCallback callback)
    {
        if (callbacks.ContainsKey(id))
        {
            callbacks[id].Remove(callback);
        }
    }

    /// <summary>
    /// Posts an event and queues it up to be executed at the next available time.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    public void Post(int id, object data = null)
    {
        if (callbacks.ContainsKey(id))
        {
            foreach (EventCallback callback in callbacks[id])
            {
                pendingEvents.Enqueue(new PendingEvent()
                {
                    EventId = id,
                    Callback = callback,
                    Data = data
                });
            }
        }
    }

    /// <summary>
    /// Posts an event and immediately executes it.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    public void PostExec(int id, object data = null)
    {
        if (callbacks.ContainsKey(id))
        {
            foreach (EventCallback callback in callbacks[id])
            {
                callback(id, data);
            }
        }
    }

    /// <summary>
    /// Executes any pending events.
    /// </summary>
    /// <param name="maxDuration">The maximum amount of time (in milliseconds) to spend executing events.  This method can run longer than this,
    /// no new events will be executed after the given time.</param>
    public void Execute(float maxDuration = 100f)
    {
        if (pendingEvents.Count > 0)
        {
            executionStopwatch.Reset();
            executionStopwatch.Start();

            while (pendingEvents.Count > 0 && executionStopwatch.ElapsedMilliseconds < maxDuration)
            {
                PendingEvent nextEvent = pendingEvents.Dequeue();
                nextEvent.Callback(nextEvent.EventId, nextEvent.Data);
            }

            executionStopwatch.Stop();
        }
    }

    #endregion Public Interface

    public void Cleanup()
    {
        pendingEvents.Clear();
    }
}

