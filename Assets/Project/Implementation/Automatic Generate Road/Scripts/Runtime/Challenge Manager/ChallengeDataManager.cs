using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;

namespace DrivingSimulation
{
    [Serializable]
    public class ChallengeData
    {
        public float RoadLength = 0;
        public float RoadWidth = 0;
        public float Turn = 0;
        public float Intersection = 0;
        public float StopSignPercentage = 0;
        public float TrafficLightPercentage = 0;

        public ChallengeData Average(ChallengeData data2)
        {
            ChallengeData avg = new ChallengeData();

            avg.RoadLength = (RoadLength + data2.RoadLength) / 2f;
            avg.RoadWidth = (RoadWidth + data2.RoadWidth) / 2f;
            avg.Turn = (Turn + data2.Turn) / 2f;
            avg.Intersection = (Intersection + data2.Intersection) / 2f;
            avg.StopSignPercentage = (StopSignPercentage + data2.StopSignPercentage) / 2f;
            avg.TrafficLightPercentage = (TrafficLightPercentage + data2.TrafficLightPercentage) / 2f;

            return avg;
        }
    }

    [Serializable]
    public struct Difficulty
    {
        public int Lane;
        public int Intersection;
    }

    public class ChallengeDataManager : SingletonDontDestroy<ChallengeDataManager>
    {

        [BoxGroup("Difficulty Data Settings")]
        public DifficultyData difficulties;
        [BoxGroup("Difficulty Data Settings")]
        [SerializeField] private int maxRecordedData = 5;
        [BoxGroup("Difficulty Data Settings")]
        [SerializeField] private Difficulty currentDifficulty;

        [BoxGroup("Calculate Settings")]
        [SerializeField] private float distancePerWrongLane = 500f;
        [BoxGroup("Calculate Settings")]
        [SerializeField] private float distancePerCrash = 500f;

        [FoldoutGroup("Debug")]
        [SerializeField, ReadOnly]
        private List<LevelDifficulty> difficultyDatas = new List<LevelDifficulty>();
        [Space(5f)]
        [FoldoutGroup("Debug")]
        [SerializeField, ReadOnly]
        private List<ChallengeData> challengeDatas = new List<ChallengeData>();
        [Space(5f)]
        [FoldoutGroup("Debug")]
        [SerializeField, ReadOnly]
        private List<PersonaData> personaDatas = new List<PersonaData>();

        private const string CURRENT_DIFFICULTY_DATA_KEY = "CurrentDifficultyData";
        private const string DIFFICULTY_DATA_KEY = "DifficultyData";
        private const string CHALLENGE_DATA_KEY = "ChallengeData";
        private const string PERSONA_DATA_KEY = "PersonaData";

        #region Props

        public int LaneDifficulty
        {
            get { return currentDifficulty.Lane; }
            set
            {
                currentDifficulty.Lane = value;

                if (currentDifficulty.Lane > difficulties.TotalLaneDifficulty())
                    currentDifficulty.Lane = difficulties.TotalLaneDifficulty();
            }
        }

        #endregion

        #region Mono

        private void OnValidate()
        {
            int maxLaneDifficulty = difficulties.RoadLengths.Count + difficulties.RoadWidths.Count + difficulties.Turns.Count;
            if (currentDifficulty.Lane > maxLaneDifficulty)
                currentDifficulty.Lane = maxLaneDifficulty;
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
            if (ES3.KeyExists(CURRENT_DIFFICULTY_DATA_KEY))
                currentDifficulty = ES3.Load<Difficulty>(CURRENT_DIFFICULTY_DATA_KEY);

            if (ES3.KeyExists(DIFFICULTY_DATA_KEY))
                difficultyDatas = ES3.Load<List<LevelDifficulty>>(DIFFICULTY_DATA_KEY);

            if (ES3.KeyExists(CHALLENGE_DATA_KEY))
                challengeDatas = ES3.Load<List<ChallengeData>>(CHALLENGE_DATA_KEY);

            if (ES3.KeyExists(PERSONA_DATA_KEY))
                personaDatas = ES3.Load<List<PersonaData>>(PERSONA_DATA_KEY);
        }

        private void SaveData()
        {
            ES3.Save(CURRENT_DIFFICULTY_DATA_KEY, currentDifficulty);
            ES3.Save(DIFFICULTY_DATA_KEY, difficultyDatas);
            ES3.Save(CHALLENGE_DATA_KEY, challengeDatas);
            ES3.Save(PERSONA_DATA_KEY, personaDatas);
        }

        #endregion

        #region Level Difficulty

        public LevelDifficulty GetLevelDifficulty()
        {
            LevelDifficulty result = new LevelDifficulty();

            // Lane Difficulty
            int remainingLaneDiffPoint = currentDifficulty.Lane;
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
            // 0 - 30%, 30% - 60%, 60 - Max
            int laneDifficultyPoint = 0;

            // Wrong lane point
            float wrongLanePercentage = newest.WrongLane * distancePerWrongLane / (currentChallenge.RoadLength) * 100;
            float wrongLaneAveragePercentage = avgPersona.WrongLane * distancePerWrongLane / (avgChallengeData.RoadLength) * 100;
            if (avgChallengeData.RoadLength == 0)
                wrongLaneAveragePercentage = 0;

            // Crash point
            float crashPercentage = newest.Crash * distancePerCrash / (currentChallenge.RoadLength) * 100;
            float crashAveragePercentage = avgPersona.Crash * distancePerCrash / (avgChallengeData.RoadLength) * 100;
            if (avgChallengeData.RoadLength == 0)
                crashAveragePercentage = 0;

            // Lane Error Percentage
            float laneErrorPercentage = (wrongLanePercentage + crashPercentage) / 2f;
            float pastLaneErrorAveragePercentage = (wrongLaneAveragePercentage + crashAveragePercentage) / 2f;

            // Increase Lane Difficulty
            if (laneErrorPercentage <= 30)
            {
                laneDifficultyPoint += CompareWithAverage(laneErrorPercentage, pastLaneErrorAveragePercentage, false);
            }

            // Decrease Lane Difficulty
            if (60 < laneErrorPercentage)
            {
                laneDifficultyPoint += CompareWithAverage(laneErrorPercentage, pastLaneErrorAveragePercentage, true);
            }

            LaneDifficulty += laneDifficultyPoint;
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
