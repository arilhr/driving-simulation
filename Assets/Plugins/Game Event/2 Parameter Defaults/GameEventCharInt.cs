using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(char, int).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Char Int Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/Char Int")]
    public class GameEventCharInt : GameEvent<char, int>
    {

    }
}
