using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, string, bool).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String String Bool Event",
        menuName = "SOGameEvents/Game Events/3 Parameter/String String Bool")]
    public class GameEventTwoStringBool : GameEvent<string, string, bool> { }
}
