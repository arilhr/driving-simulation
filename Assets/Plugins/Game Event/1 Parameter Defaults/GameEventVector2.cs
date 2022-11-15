using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (Vector2).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Vector2 Event",
        menuName = "SOGameEvents/Game Events/1 Parameter/Vector2")]
    public class GameEventVector2 : GameEvent<Vector2>
    {

    }
}
