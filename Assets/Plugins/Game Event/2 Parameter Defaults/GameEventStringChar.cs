using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, char).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Char Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/String Char")]
    public class GameEventStringChar : GameEvent<string, char>
    {

    }
}
