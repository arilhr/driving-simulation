using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (bool).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Bool Event",
        menuName = "SOGameEvents/Game Events/1 Parameter/Boolean")]
    public class GameEventBool : GameEvent<bool>
    {

    }
}
