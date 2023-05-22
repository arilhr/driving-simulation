using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;

namespace DrivingSimulation
{
    [Serializable]
    public class LevelDifficulty
    {
        public int RoadLength;
        public int RoadWidth;
        public int Turn;
    }

    [Serializable]
    public class ChallengeData
    {
        public float RoadLength;
        public float RoadWidth;
        public float Turn;

        public ChallengeData Average(ChallengeData data2)
        {
            ChallengeData avg = new ChallengeData();

            avg.RoadLength = (RoadLength + data2.RoadLength) / 2f;
            avg.RoadWidth = (RoadWidth + data2.RoadWidth) / 2f;
            avg.Turn = (Turn + data2.Turn) / 2f;

            return avg;
        }
    }

    public class ChallengeDataManager : Singleton<ChallengeDataManager>
    {
        [Header("Difficulty Data List")]
        public DifficultyData difficulties;

        [Header("Current Difficulty")]
        [SerializeField] private int laneDifficulty = 0;
        [SerializeField] private int intersectionDifficulty = 0;

        [Header("Settings")]
        [SerializeField] private int maxRecordedData = 5;

        [Space(10f)]

        [BoxGroup("Debug")]
        [SerializeField, ReadOnly]
        private List<LevelDifficulty> difficultyDatas = new List<LevelDifficulty>();
        [Space(5f)]
        [BoxGroup("Debug")]
        [SerializeField, ReadOnly]
        private List<ChallengeData> challengeDatas = new List<ChallengeData>();
        [Space(5f)]
        [BoxGroup("Debug")]
        [SerializeField, ReadOnly]
        private List<PersonaData> personaDatas = new List<PersonaData>();

        private const string CHALLENGE_DATA_KEY = "ChallengeData";
        private const string PERSONA_DATA_KEY = "PersonaData";

        #region Mono

        private void OnValidate()
        {
            int maxLaneDifficulty = difficulties.RoadLengths.Count + difficulties.RoadWidths.Count + difficulties.Turns.Count;
            if (laneDifficulty > maxLaneDifficulty)
                laneDifficulty = maxLaneDifficulty;
        }

        protected override void Awake()
        {
            base.Awake();

            LoadData();
        }

        #endregion

        #region Method

        private void LoadData()
        {
            if (ES3.KeyExists(CHALLENGE_DATA_KEY))
                challengeDatas = ES3.Load<List<ChallengeData>>(CHALLENGE_DATA_KEY);

            if (ES3.KeyExists(PERSONA_DATA_KEY))
                personaDatas = ES3.Load<List<PersonaData>>(PERSONA_DATA_KEY);
        }

        private void SaveData()
        {
            ES3.Save(CHALLENGE_DATA_KEY, challengeDatas);
            ES3.Save(PERSONA_DATA_KEY, personaDatas);
        }

        #endregion

        #region Level Difficulty

        public LevelDifficulty GetLevelDifficulty()
        {
            LevelDifficulty result = new LevelDifficulty();

            // Lane Difficulty
            int remainingLaneDiffPoint = laneDifficulty;
            while (remainingLaneDiffPoint > 0)
            {
                // 0: road length, 1: road width, 2: turns
                List<int> challengePool = new List<int>{ 0, 1, 2 };
                if (result.RoadLength >= difficulties.RoadLengths.Count - 1)
                    challengePool.Remove(0);
                if (result.RoadWidth >= difficulties.RoadWidths.Count - 1)
                    challengePool.Remove(1);
                if (result.Turn >= difficulties.Turns.Count - 1)
                    challengePool.Remove(2);

                if (challengePool.Count <= 0)
                    break;

                int selectedLaneDiff = challengePool[Random.Range(0, challengePool.Count)];
                if (selectedLaneDiff == 0)
                    result.RoadLength++;
                if (selectedLaneDiff == 1)
                    result.RoadWidth++;
                if (selectedLaneDiff == 2)
                    result.Turn++;

                remainingLaneDiffPoint--;
            }

            return result;
        }

        public ChallengeData GenerateChallengeData(LevelDifficulty diffToGenerate)
        {
            if (difficulties == null)
            {
                Debug.Log("Difficulties data is null!");
                return null;
            }

            ChallengeData result = new ChallengeData();

            // Road Length
            if (difficulties.RoadLengths.Count <= 0)
            {
                Debug.Log("Road length difficulty in null!");
                return null;
            }

            if (diffToGenerate.RoadLength > difficulties.RoadLengths.Count - 1)
                diffToGenerate.RoadLength = difficulties.RoadLengths.Count - 1;

            float roadLengthResult = Random.Range(difficulties.RoadLengths[diffToGenerate.RoadLength].Min, difficulties.RoadLengths[diffToGenerate.RoadLength].Max);
            result.RoadLength = roadLengthResult;

            // Road widths
            if (difficulties.RoadWidths.Count <= 0)
            {
                Debug.Log("Road widths difficulty in null!");
                return null;
            }

            if (diffToGenerate.RoadWidth > difficulties.RoadWidths.Count - 1)
                diffToGenerate.RoadWidth = difficulties.RoadWidths.Count - 1;

            float roadWidthResult = Random.Range(difficulties.RoadWidths[diffToGenerate.RoadWidth].Min, difficulties.RoadWidths[diffToGenerate.RoadWidth].Max);
            result.RoadWidth = roadWidthResult;

            // Turns
            if (difficulties.Turns.Count <= 0)
            {
                Debug.Log("Turns difficulty in null!");
                return null;
            }

            if (diffToGenerate.Turn > difficulties.Turns.Count - 1)
                diffToGenerate.Turn = difficulties.Turns.Count - 1;

            float turnsResult = Random.Range(difficulties.Turns[diffToGenerate.Turn].Min, difficulties.Turns[diffToGenerate.Turn].Max);
            result.Turn = turnsResult;

            return result;
        }

        public ChallengeData AverageChallengeData()
        {
            ChallengeData result = new ChallengeData();

            for (int i = 0; i < challengeDatas.Count; i++)
            {
                if (i == 0)
                {
                    result = challengeDatas[i];
                    continue;
                }

                result = result.Average(challengeDatas[i]);
            }

            return result;
        }

        public void CalculateDifficulty(ChallengeData currentChallenge, PersonaData newest)
        {
            PersonaData avgPersona = AveragePersonaData();
            ChallengeData avgChallengeData = AverageChallengeData();

            // LANE DIFFICULTY
            int laneDifficultyPoint = 0;

            // Wrong lane point
            // 0 - 30%, 30% - 60%, 60 - Max
            float distancePerWrong = 300;
            float wrongLanePercentage = newest.WrongLane * distancePerWrong / (currentChallenge.RoadLength) * 100;
            float wrongLaneAveragePercentage = avgPersona.WrongLane * distancePerWrong / (avgChallengeData.RoadLength) * 100;
            if (wrongLanePercentage <= 30)
            {
                laneDifficultyPoint += CompareWithAverage(wrongLanePercentage, wrongLaneAveragePercentage, false);
            }

            if (60 < wrongLanePercentage)
            {
                laneDifficultyPoint += CompareWithAverage(wrongLanePercentage, wrongLaneAveragePercentage, true);
            }

            // WRONG 
        }

        private int CompareWithAverage(float percentage, float average, bool isNegative)
        {
            if (isNegative)
            {
                if (percentage > average)
                    return -2;
                if (percentage <= average)
                    return -1;
            }

            if (percentage < average)
                return 1;
            if (percentage >= average)
                return 2;

            return 0;
        }

        public void AddDifficultyData(LevelDifficulty diff, ChallengeData challenge)
        {
            if (diff == null || challenge == null)
            {
                Debug.Log("Difficulty or Challenge data must be not null.");
                return;
            }

            challengeDatas.Add(challenge);
            if (challengeDatas.Count > maxRecordedData)
                challengeDatas.RemoveAt(challengeDatas.Count - 1);

            difficultyDatas.Add(diff);
            if (difficultyDatas.Count > maxRecordedData)
                difficultyDatas.RemoveAt(difficultyDatas.Count - 1);

            SaveData();
        }


        #endregion

        #region Persona Data Method

        public void AddLatestData(PersonaData newValue)
        {
            personaDatas.Add(newValue);

            if (personaDatas.Count > maxRecordedData)
            {
                personaDatas.RemoveAt(maxRecordedData - 1);
            }
        }

        public PersonaData AveragePersonaData()
        {
            PersonaData avg = new PersonaData();

            for (int i = 0; i < personaDatas.Count; i++)
            {
                if (i == 0)
                {
                    avg = personaDatas[0];
                    continue;
                }

                avg = avg.Average(personaDatas[i]);
            }

            return avg;
        }

        public void AddPersonaData(PersonaData persona)
        {
            if (persona == null)
            {
                Debug.Log("Persona data must not null.");
                return;
            }

            personaDatas.Add(persona);
            if (personaDatas.Count > maxRecordedData)
                personaDatas.RemoveAt(personaDatas.Count - 1);

            SaveData();
        }


        #endregion
    }
}
