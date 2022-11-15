using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(char, float).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Char Float Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/Char Float")]
    public class GameEventCharFloat : GameEvent<char, float>
    {

    }
}
