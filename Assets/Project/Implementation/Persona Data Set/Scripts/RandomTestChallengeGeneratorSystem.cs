using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

namespace DrivingSimulation
{
    public class RandomTestChallengeGeneratorSystem : MonoBehaviour
    {
        #region Data

        public List<DummyChallengeData> trainingChallengeData = new();

        [Header("UI")]
        [SerializeField] private UI_ChallengePersonaGroup _challengePersonaGroupUI = null;
        [SerializeField] private Transform _challengeListContentParent = null;

        [Header("Average UI")]
        [SerializeField] private UI_PersonaData _averagePersonaDataUI = null;
        [SerializeField] private UI_ChallengeData _averageChallengeDataUI = null;

        [Header("Next Challenge Generated UI")]
        [SerializeField] private UI_PersonaData _nextChallengePersonaDataUI = null;
        [SerializeField] private UI_ChallengeData _nextChallengeChallengeDataUI = null;

        private List<ChallengeGeneratedData> _challengeListOrder = new();
        private Dictionary<ChallengeGeneratedData, PersonaDataset> _personaDatasetLists = new();

        // Average Data
        private PersonaDataset _averagePersonaData = null;
        private ChallengeGeneratedData _averageChallengeGeneratedData = null;

        // Newest Generated Next Challenge
        private ChallengeGeneratedData _nextChallengeGenerated = null;
        private PersonaDataset _personaDatasetNextChallenge = null;

        private List<UI_ChallengePersonaGroup> _challengeListUIs = new();

        private int _maxPastChallenge = 5;

        #endregion

        #region Mono

        private void Start()
        {
            GenerateTrainingPersonaData();
        }

        #endregion

        #region Generate

        public void GenerateTrainingPersonaData()
        {
            // Generate persona data from dummy challenge data
            foreach (DummyChallengeData dummyChallengeData in trainingChallengeData)
            {
                PersonaDataset newPersonalDataset = GeneratePersonaData(dummyChallengeData.data);

                AddPersonaData(dummyChallengeData.data, newPersonalDataset);
            }

            UpdateListUI();
        }

        public PersonaDataset GeneratePersonaData(ChallengeGeneratedData data)
        {
            PersonaDataset result = new();

            // wrong lane generated
            result.WrongLane = Mathf.RoundToInt(Random.Range(0, (data.LongRoad / (data.RoadSize * 10f)))) * data.RoadSize / 20f;
            result.Crash = Mathf.RoundToInt(Random.Range(0, (data.LongRoad / (data.RoadSize * 50f))));

            // indicator
            float wrongIndicatorPercentage = Random.Range(0, 1f);
            result.WrongIndicator = Mathf.Ceil(wrongIndicatorPercentage * data.TotalIntersect);
            result.CorrectIndicator = data.TotalIntersect - result.WrongIndicator;

            // stop sign
            float stopSignPercentage = Random.Range(0, 1f);
            result.WrongStopSign = Mathf.Ceil(stopSignPercentage * data.TotalStopSign);
            result.CorrectStopSign = data.TotalStopSign - result.WrongStopSign;

            // traffic light percentage
            float trafficLightPercentage = Random.Range(0, 1f);
            result.WrongTrafficLight = Mathf.Ceil(trafficLightPercentage * data.TotalTrafficLight);
            result.CorrectTrafficLight = data.TotalTrafficLight - result.WrongTrafficLight;

            // speed limit percentage
            float maxSpeedLimitPercentage = Random.Range(0, 1f);
            result.ViolateMaxSpeed = Mathf.Ceil(maxSpeedLimitPercentage * data.TotalMaxSpeedLimit);
            result.PassMaxSpeed = data.TotalMaxSpeedLimit;

            return result;
        }

        private void AddPersonaData(ChallengeGeneratedData challengeData, PersonaDataset personaData)
        {
            _challengeListOrder.Add(challengeData);
            _personaDatasetLists.Add(challengeData, personaData);

            if (_personaDatasetLists.Count > _maxPastChallenge)
            {
                ChallengeGeneratedData firstIndexDataKey = _challengeListOrder[0];
                _personaDatasetLists.Remove(firstIndexDataKey);
                _challengeListOrder.RemoveAt(0);
            }

            CountAverageData();

            UpdateListUI();
        }

        private void CountAverageData()
        {
            // Reset data
            _averagePersonaData = null;
            _averageChallengeGeneratedData = null;

            foreach (KeyValuePair<ChallengeGeneratedData, PersonaDataset> kvp in _personaDatasetLists)
            {
                // Count challenge data average
                if (_averageChallengeGeneratedData == null)
                {
                    _averageChallengeGeneratedData = new ChallengeGeneratedData(kvp.Key);
                }
                else
                {
                    _averageChallengeGeneratedData.Average(kvp.Key);
                }

                // Count persona data average
                if (_averagePersonaData == null)
                {
                    _averagePersonaData = new PersonaDataset(kvp.Value);
                }
                else
                {
                    _averagePersonaData.Average(kvp.Value);
                }

            }
            
            // Update UI
            UpdateAveragePersonaUI();
            UpdateAverageChallengeDataUI();
        }

        private void GenerateNextChallenge()
        {
            _nextChallengeGenerated = ChallengeGeneratorSystem.GenerateChallenge(_averageChallengeGeneratedData, _averagePersonaData);

            _personaDatasetNextChallenge = GeneratePersonaData(_nextChallengeGenerated);

            AddPersonaData(_nextChallengeGenerated, _personaDatasetNextChallenge);
            UpdateNextChallengeUI();
        }

        #endregion

        #region UI Methods

        private void UpdateListUI()
        {
            // Deactive all current challenge list UI
            foreach (UI_ChallengePersonaGroup ui in _challengeListUIs)
            {
                ui.gameObject.SetActive(false);
            }

            // Iterate all current persona datas
            List<UI_ChallengePersonaGroup> newUI = new();
            int i = 0;
            foreach (ChallengeGeneratedData dataKey in _challengeListOrder)
            {
                if (i < _challengeListUIs.Count)
                {
                    _challengeListUIs[i].Initialize(dataKey, _personaDatasetLists[dataKey], true, i + 1);
                    _challengeListUIs[i].gameObject.SetActive(true);
                }
                else
                {
                    UI_ChallengePersonaGroup _newChallengeListUI = Instantiate(_challengePersonaGroupUI, _challengeListContentParent);
                    _newChallengeListUI.Initialize(dataKey, _personaDatasetLists[dataKey], true, i + 1);
                    newUI.Add(_newChallengeListUI);
                }

                i++;
            }

            // Add all new UI to List
            _challengeListUIs.AddRange(newUI);
        }

        private void UpdateAveragePersonaUI()
        {
            _averagePersonaDataUI.Initialize(_averagePersonaData);
        }

        private void UpdateAverageChallengeDataUI()
        {
            _averageChallengeDataUI.Initialize(_averageChallengeGeneratedData);
        }

        public void GenerateNextChallengeCallback()
        {
            GenerateNextChallenge();
        }

        private void UpdateNextChallengeUI()
        {
            _nextChallengeChallengeDataUI.Initialize(_nextChallengeGenerated);
            _nextChallengePersonaDataUI.Initialize(_personaDatasetNextChallenge);
        }

        #endregion
    }
}
