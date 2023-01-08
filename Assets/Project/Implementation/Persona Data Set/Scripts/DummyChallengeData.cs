using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    [CreateAssetMenu(fileName = "New Dummy Challenge Data", menuName = "Persona/New Dummy Challenge Data")]
    public class DummyChallengeData : ScriptableObject
    {
        public ChallengeGeneratedData data;
    }
}
