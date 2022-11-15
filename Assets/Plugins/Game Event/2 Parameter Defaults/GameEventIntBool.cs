using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(int, bool).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Bool Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/Int Bool")]
    public class GameEventIntBool : GameEvent<int, bool>
    {

    }
}
