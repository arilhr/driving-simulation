using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 4 parameters(string, string, int, bool).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String String Int Bool Event",
        menuName = "SOGameEvents/Game Events/4 Parameter/String String Int Bool")]
    public class GameEventTwoStringIntBool : GameEvent<string, string, int, bool>
    {

    }
}
