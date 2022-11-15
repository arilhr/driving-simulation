using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, int, Vector2).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Int Int Event",
        menuName = "SOGameEvents/Game Events/3 Parameter/String Int Vector2")]
    public class GameEventStringIntVector2 : GameEvent<string, int, Vector2>
    {

    }
}
