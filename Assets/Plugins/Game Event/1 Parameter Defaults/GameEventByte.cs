using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (byte).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Byte Event",
        menuName = "SOGameEvents/Game Events/1 Parameter/Byte")]
    public class GameEventByte : GameEvent<byte>
    {

    }
}
