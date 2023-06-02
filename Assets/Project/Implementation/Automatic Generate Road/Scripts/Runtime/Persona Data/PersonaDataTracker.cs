using SOGameEvents;
using UnityEngine;

namespace DrivingSimulation
{
    public class PersonaDataTracker : Singleton<PersonaDataTracker>
    {
        private PersonaData personaData = new PersonaData();

        [Header("Events")]
        public GameEventNoParam OnGameEndCallback = null;

        public PersonaData PersonaData
        {
            get
            {
                return personaData;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            // OnGameEndCallback?.AddListener(Save);
        }

        private void OnDestroy()
        {
            // OnGameEndCallback?.RemoveListener(Save);
        }

        public void Save()
        {
            if (ChallengeDataManager.Instance == null)
            {
                Debug.Log($"Challenge data manager is not found!");
                return;
            }

            ChallengeDataManager.Instance.AddPersonaData(personaData);
        }
    }
}
