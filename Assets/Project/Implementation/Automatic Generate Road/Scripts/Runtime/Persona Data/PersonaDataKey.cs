using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    [CreateAssetMenu(fileName = "New Persona Data Key", menuName = "Persona Data/New Persona Data Key", order = 0)]
    public class PersonaDataKey : ScriptableObject
    {
        public List<string> Keys;
    }
}
