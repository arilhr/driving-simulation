using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, byte).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Byte Event",
        menuName = "SOGameEvents/Game Events/2 Parameter/String Byte")]
    public class GameEventStringByte : GameEvent<string, byte>
    {

    }
}
