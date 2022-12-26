using UnityEngine;

namespace SOGameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(float, float, float).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Float Float Float Event",
        menuName = "SOGameEvents/Game Events/3 Parameter/Float Float Float")]
    public class GameEventThreeFloat : GameEvent<float, float, float>
    {

    }
}
