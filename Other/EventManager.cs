using System;
using System.Collections.Generic;

public abstract class GameEvent { }

public class EventManager
{
    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            return instance = instance ?? new EventManager();
        }
    }

	public delegate void Listener<T>(T gameEvent) where T : GameEvent;

    private readonly Dictionary<Type, List<Delegate>> listeners =
        new Dictionary<Type, List<Delegate>>();

    /// <summary>
    /// Adds a listener for a specific GameEvent type.
    /// </summary>
    /// <typeparam name="T">Type of the GameEvent</typeparam>
    /// <param name="listener">The listener you would like to add.</param>
    public void Subscribe<T>(Listener<T> listener) where T : GameEvent
    {
        Type type = typeof(T);
        if (listeners.ContainsKey(type))
        {
            listeners[type].Add(listener);
        }
        else
        {
            listeners.Add(type, new List<Delegate>() { listener });
        }
    }

    /// <summary>
    /// Attempts to remove a specific listener from the EventManager.
    /// </summary>
    /// <typeparam name="T">Type of the GameEvent</typeparam>
    /// <param name="listener">The listener you would like to remove.</param>
    /// <returns>True if something was removed. False otherwise.</returns>
    public bool Unsubscribe<T>(Listener<T> listener) where T : GameEvent
    {
        List<Delegate> list = null;
        if (listeners.TryGetValue(typeof(T), out list))
        {
            list.Remove(listener);
            if (list.Count == 0)
            {
                listeners.Remove(typeof(T));
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Attempts to raise an event. Runs every listener for that event.
    /// </summary>
    /// <typeparam name="T">Type of the GameEvent</typeparam>
    /// <param name="listener">The GameEvent you would like to raise</param>
    /// <returns>True if a listener was called. False otherwise.</returns>
    public bool Raise<T>(T gameEvent) where T : GameEvent
    {
        List<Delegate> list = null;
        if (listeners.TryGetValue(typeof(T), out list))
        {
            foreach (Listener<T> listener in list)
            {
                listener(gameEvent);
            }
            return true;
        }
        return false;
    }
}
