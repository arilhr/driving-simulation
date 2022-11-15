using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, long).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Long Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/String Long")]
    public class GameEventStringLong : GameEvent<string, long>
    {

    }
}
