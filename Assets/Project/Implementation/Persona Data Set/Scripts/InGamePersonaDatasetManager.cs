using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace DrivingSimulation
{
    public class InGamePersonaDatasetManager : MonoBehaviour
    {
        public static InGamePersonaDatasetManager Instance = null;

        [Space(20f)]
        [SerializeField, ReadOnly]
        private PersonaDataset _gamePersonaDataset = new();

        public PersonaDataset GamePersonaDataset
        {
            get { return _gamePersonaDataset; }
        }

        #region Mono

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        #endregion

        #region Method

        public void WrongStopSign()
        {
            _gamePersonaDataset.WrongStopSign += 1;
        }

        public void CorrectStopSign()
        {
            _gamePersonaDataset.CorrectStopSign += 1;
        }

        public void WrongTrafficLight()
        {
            _gamePersonaDataset.WrongTrafficLight += 1;
        }

        public void CorrectTrafficLight()
        {
            _gamePersonaDataset.CorrectTrafficLight += 1;
        }

        public void WrongLane(float laneSize)
        {
            _gamePersonaDataset.WrongLane += laneSize / 100f;
        }

        public void SetLongRoad(float longRoad)
        {
            //_gamePersonaDataset.LongRoad = longRoad;
        }

        public void AddTotalIntersect()
        {
            //_gamePersonaDataset.TotalIntersect += 1;
        }

        public void ViolateMaxSpeed()
        {
            _gamePersonaDataset.ViolateMaxSpeed += 1;
        }

        public void ViolateMinSpeed()
        {
            // _gamePersonaDataset.ViolateMinSpeed += 1;
        }

        public void Crash()
        {
            _gamePersonaDataset.Crash += 1;
        }

        public void WrongIndicator()
        {
            _gamePersonaDataset.WrongIndicator += 1;
        }

        public void CorrectIndicator()
        {
            _gamePersonaDataset.CorrectIndicator += 1;
        }

        public void OnGameEnd()
        {
            Debug.Log($"Add new persona data!");
            PersonaDatasetManager.AddNewGamePersona(_gamePersonaDataset);
        }

        #endregion
    }
}
