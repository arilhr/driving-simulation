using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (char).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Char Event",
        menuName = "SOGameEvents/Game Events/1 Parameter/Char")]
    public class GameEventChar : GameEvent<char> { }
}
