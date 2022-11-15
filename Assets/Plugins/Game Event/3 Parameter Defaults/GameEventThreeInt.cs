using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(int, int, int).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Int Int Event",
        menuName = "SOGameEvents/Game Events/3 Parameter/Int Int Int")]
    public class GameEventThreeInt : GameEvent<int, int, int>
    {

    }
}
