using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(int, GameObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Transform Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/Int Transform")]
    public class GameEventIntTransform : GameEvent<int, Transform>
    {

    }
}
