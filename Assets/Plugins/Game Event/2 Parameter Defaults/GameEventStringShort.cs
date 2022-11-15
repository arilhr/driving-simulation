using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, short).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Short Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/String Short")]
    public class GameEventStringShort : GameEvent<string, short>
    {

    }
}
