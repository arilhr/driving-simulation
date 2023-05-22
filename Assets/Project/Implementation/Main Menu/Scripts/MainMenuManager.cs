using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DrivingSimulation
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private UI_Level _levelPrefab;
        [SerializeField]
        private Transform _levelContainer;
        
        [Header("Events")]
        [SerializeField]
        private GameEventInt _gotoLevelCallback = null;
    }
}
