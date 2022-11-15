using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, string).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String String Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/String String")]
    public class GameEventTwoString : GameEvent<string, string>
    {

    }
}
