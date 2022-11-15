using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, Vector3).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Vector3 Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/String Vector3")]
    public class GameEventStringVector3 : GameEvent<string, Vector3>
    {

    }
}
