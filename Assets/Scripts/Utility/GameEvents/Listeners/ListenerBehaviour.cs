using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ListenerBehaviour<T, E> : MonoBehaviour, IGameEventListener<T>
    where E : BaseGameEvent<T>
{
    protected List<E> gameEvents;

    public List<E> GameEvents { get => gameEvents; set => gameEvents = value; }

    private void OnEnable()
    {
        foreach(E gameEvent in GameEvents)
            if(gameEvent != null)
                gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        foreach (E gameEvent in GameEvents)
            if (gameEvent != null)
                gameEvent.UnregisterListener(this);
    }

    public abstract void OnEventRaised(T item);
}
