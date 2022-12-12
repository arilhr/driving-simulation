using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{

    [Serializable]
    public struct SceneData
    {
        [Scene]
        public string scene;
    }

    [CreateAssetMenu(fileName = "Level Data", menuName = "Level")]
    public class LevelData : ScriptableObject
    {
        public List<SceneData> _levelScenes;
    }

}

