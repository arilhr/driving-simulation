using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (Collider2D).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Collider2D Event",
        menuName = "SOGameEvents/Game Events/1 Parameter/Collider2D")]
    public class GameEventCollider2D : GameEvent<Collider2D>
    {

    }
}
