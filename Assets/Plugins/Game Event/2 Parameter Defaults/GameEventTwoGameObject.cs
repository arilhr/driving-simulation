using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(GameObject, GameObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New GameObject GameObject Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/GameObject GameObject")]
    public class GameEventTwoGameObject : GameEvent<GameObject, GameObject>
    {

    }
}
