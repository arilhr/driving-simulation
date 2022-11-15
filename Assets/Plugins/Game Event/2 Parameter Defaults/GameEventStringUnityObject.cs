using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, GameObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String UnityObject Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/String UnityObject")]
    public class GameEventStringUnityObject : GameEvent<string, Object>
    {
        
    }
}
