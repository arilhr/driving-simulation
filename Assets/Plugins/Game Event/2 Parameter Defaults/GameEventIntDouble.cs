using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(int, double).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Double Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/Int Double")]
    public class GameEventIntDouble : GameEvent<int, double>
    {

    }
}
