using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (int).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Event",
        menuName = "SOGameEvents/Game Events/1 Parameter/Int")]
    public class GameEventInt : GameEvent<int>
    {

    }
}
