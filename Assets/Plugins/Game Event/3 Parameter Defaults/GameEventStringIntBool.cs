using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, int, bool).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Int Bool Event",
        menuName = "SOGameEvents/Game Events/3 Parameter/String Int Bool")]
    public class GameEventStringIntBool : GameEvent<string, int, bool> { }
}
