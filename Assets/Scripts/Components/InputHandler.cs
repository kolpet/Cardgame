using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    private IDictionary<int, Interaction> _interactions = new Dictionary<int, Interaction>();

    public Interaction this[int index] { get => _interactions.ContainsKey(index) ? _interactions[index] : null; }

    public bool ContainsInteraction(int id) { return _interactions.ContainsKey(id); }

    public Interaction GetInteraction(int id) { return _interactions[id]; }

    public IEnumerable<KeyValuePair<int, Interaction>> GetInteractions() { return _interactions; }

    private void Update()
    {
        _interactions.Clear();
        MouseInteractions();
        TouchInteractions();
    }
    void MouseInteractions()
    {
        for(int i = 0; i < 2; i++)
        {
            if (Input.GetMouseButton(i))
            {
                InteractionPhase phase = InteractionPhase.Moved;
                if (Input.GetMouseButtonDown(i)) 
                    phase = InteractionPhase.Began;
                if (Input.GetMouseButtonUp(i))
                    phase = InteractionPhase.Ended;
                _interactions[i] = new Interaction
                {
                    position = Camera.main.ScreenToWorldPoint(Input.mousePosition),
                    phase = phase,
                    type = InteractionType.Touch,
                    key = i
                };
            }
        }
    }

    private static Dictionary<TouchPhase, InteractionPhase> converter = new Dictionary<TouchPhase, InteractionPhase>
        {
            { TouchPhase.Began, InteractionPhase.Began },
            { TouchPhase.Moved, InteractionPhase.Moved },
            { TouchPhase.Stationary, InteractionPhase.Moved },
            { TouchPhase.Ended, InteractionPhase.Ended },
            { TouchPhase.Canceled, InteractionPhase.Ended },
        };

    void TouchInteractions()
    {
        Touch[] touches = Input.touches;
        //Check each touch point
        foreach (Touch touch in touches)
        {
            //Create new InteractionData
            _interactions[touch.fingerId] = new Interaction
            {
                position = Camera.main.ScreenToWorldPoint(touch.position),
                phase = converter[touch.phase],
                type = InteractionType.Touch,
                key = touch.fingerId
            };
        }
    }
}

public class Interaction
{
    public Vector2 position;
    public InteractionPhase phase;
    public InteractionType type;
    public int key;
}

public enum InteractionPhase
{
    Began,
    Moved,
    Ended,
    None
}

public enum InteractionType
{
    Mouse,
    Touch
}
