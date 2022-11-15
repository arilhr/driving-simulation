using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, int).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Int Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/String Int")]
    public class GameEventStringInt : GameEvent<string, int>
    {

    }
}
