using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 4 parameters(string, string, string, string).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String String String String Event",
        menuName = "SOGameEvents/Game Events/4 Parameter/String String String String")]
    public class GameEventFourString : GameEvent<string, string, string, string>
    {

    }
}
