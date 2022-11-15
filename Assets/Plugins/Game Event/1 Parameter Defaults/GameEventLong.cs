using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (long).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Long Event",
        menuName = "SOGameEvents/Game Events/1 Parameter/Long")]
    public class GameEventLong : GameEvent<long>
    {

    }
}
