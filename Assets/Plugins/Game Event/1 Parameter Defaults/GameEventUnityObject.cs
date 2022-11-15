using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (Object).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New UnityObject Event",
        menuName = "SOGameEvents/Game Events/1 Parameter/UnityObject")]
    public class GameEventUnityObject : GameEvent<Object>
    {

    }
}
