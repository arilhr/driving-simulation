using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(int, Transform, int).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Transform Int Event",
        menuName = "SOGameEvents/Game Events/3 Parameter/Int Transform Int")]
    public class GameEventIntTransformInt : GameEvent<int, Transform, int> { }
}
