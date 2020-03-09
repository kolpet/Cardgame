using UnityEngine;

[CreateAssetMenu(fileName = "New Game Event", menuName = "Events/New Void Event")]
public class VoidEvent : BaseGameEvent<Void>
{
    public void Raise() => Raise(new Void());
}