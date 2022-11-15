using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, string, int).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String String Int Event",
        menuName = "SOGameEvents/Game Events/3 Parameter/String String Int")]
    public class GameEventTwoStringInt : GameEvent<string, string, int>
    {

    }
}
