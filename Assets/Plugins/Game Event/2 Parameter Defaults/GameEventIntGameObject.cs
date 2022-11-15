using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(int, GameObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int GameObject Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/Int GameObject")]
    public class GameEventIntGameObject : GameEvent<int, GameObject>
    {

    }
}
