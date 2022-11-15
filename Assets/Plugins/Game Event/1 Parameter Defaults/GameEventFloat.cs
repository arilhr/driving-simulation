using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (float).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Float Event",
        menuName = "SOGameEvents/Game Events/1 Parameter/Float")]
    public class GameEventFloat : GameEvent<float>
    {

    }
}
