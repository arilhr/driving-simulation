using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (short).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Short Event",
        menuName = "SOGameEvents/Game Events/1 Parameter/Short")]
    public class GameEventShort : GameEvent<short>
    {

    }
}
