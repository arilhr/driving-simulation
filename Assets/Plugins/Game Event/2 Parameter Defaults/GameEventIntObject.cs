using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(int, object).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Object Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/Int Object")]
    public class GameEventIntObject : GameEvent<int, object>
    {

    }
}
