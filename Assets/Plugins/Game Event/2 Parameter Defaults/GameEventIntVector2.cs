using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(int, Vector2).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Vector2 Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/Int Vector2")]
    public class GameEventIntVector2 : GameEvent<int, Vector2>
    {

    }
}
