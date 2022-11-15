using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, GameObject, GameObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String GameObject GameObject Event",
        menuName = "SOGameEvents/Game Events/3 Parameter/String GameObject GameObject")]
    public class GameEventStringTwoGameObject : GameEvent<string, GameObject, GameObject>
    {

    }
}
