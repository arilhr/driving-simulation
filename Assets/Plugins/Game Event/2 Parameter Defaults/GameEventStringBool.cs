using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, bool).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Bool Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/String Bool")]
    public class GameEventStringBool : GameEvent<string, bool>
    {

    }
}
