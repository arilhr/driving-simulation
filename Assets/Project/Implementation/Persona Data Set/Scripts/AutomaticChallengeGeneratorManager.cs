using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class AutomaticChallengeGeneratorManager : SingletonDontDestroy<AutomaticChallengeGeneratorManager>
    {
        [Header("Setting")]
        public int numOfPastChallengeCount = 5;

        [Header("Persona Data")]
        public List<PersonaDataset> personaDatasetList;
        public PersonaDataset averagePersonaDataset;

        [Header("Challenge Data")]
        public List<ChallengeGeneratedData> challengeDataList;
        public ChallengeGeneratedData averageChallengeData;

        private const string PERSONA_DATA_KEY = "PersonaData";
        private const string CHALLENGE_DATA_KEY = "ChallengeData";

        #region Internal Method

        protected override void Awake()
        {
            base.Awake();

            // Load Data
            LoadData();
            CountAverage();
        }

        void SaveData()
        {
            ES3.Save(PERSONA_DATA_KEY, personaDatasetList);
            ES3.Save(CHALLENGE_DATA_KEY , challengeDataList);
        }

        void LoadData()
        {
            if (ES3.KeyExists(PERSONA_DATA_KEY))
                personaDatasetList = ES3.Load<List<PersonaDataset>>(PERSONA_DATA_KEY);
            if (ES3.KeyExists(CHALLENGE_DATA_KEY))
                challengeDataList = ES3.Load<List<ChallengeGeneratedData>>(CHALLENGE_DATA_KEY);
        }

        void CountAverage()
        {
            foreach (PersonaDataset persona in personaDatasetList)
            {
                averagePersonaDataset.Average(persona);
            }

            foreach (ChallengeGeneratedData challengeGeneratedData in challengeDataList)
            {
                averageChallengeData.Average(challengeGeneratedData);
            }
        }

        #endregion
    }
}
