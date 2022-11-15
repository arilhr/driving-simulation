using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(int, float).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Float Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/Int Float")]
    public class GameEventIntFloat : GameEvent<int, float>
    {

    }
}
