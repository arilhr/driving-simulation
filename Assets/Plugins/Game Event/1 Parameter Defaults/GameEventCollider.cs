using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (Collider).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Collider Event",
        menuName = "SOGameEvents/Game Events/1 Parameter/Collider")]
    public class GameEventCollider : GameEvent<Collider>
    {

    }
}
