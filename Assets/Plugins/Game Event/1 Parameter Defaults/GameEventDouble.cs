using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (double).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Double Event",
        menuName = "SOGameEvents/Game Events/1 Parameter/Double")]
    public class GameEventDouble : GameEvent<double>
    {

    }
}
