using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, GameObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String GameObject Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/String GameObject")]
    public class GameEventStringGameObject : GameEvent<string, GameObject>
    {

    }
}
