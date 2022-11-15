using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter(Collision2D).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Collision2D Event",
        menuName = "SOGameEvents/Game Events/1 Parameter/Collision2D")]
    public class GameEventCollision2D : GameEvent<Collision2D>
    {

    }
}
