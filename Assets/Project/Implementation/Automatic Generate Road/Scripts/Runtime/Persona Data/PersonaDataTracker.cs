using Newtonsoft.Json;
using SOGameEvents;
using UnityEngine;

namespace DrivingSimulation
{
    public class PersonaDataTracker : Singleton<PersonaDataTracker>
    {
        private PersonaDataValue personaData = new PersonaDataValue();

        [Header("Events")]
        public GameEventNoParam OnGameEndCallback = null;

        protected override void Awake()
        {
            base.Awake();

            OnGameEndCallback?.AddListener(Save);
        }

        private void OnDestroy()
        {
            OnGameEndCallback?.RemoveListener(Save);
        }

        public void Add(string key)
        {
            if (personaData.Value.ContainsKey(key))
            {
                personaData.Value[key] += 1;
                return;
            }

            personaData.Value.Add(key, 1);
        }

        public void Save()
        {

            string jsonPersona = JsonConvert.SerializeObject(personaData.Value);

            Debug.Log(jsonPersona);

            if (PersonaDataManager.Instance == null)
            {
                Debug.Log($"Persona data manager is not found!");
                return;
            }

            PersonaDataManager.Instance.AddLatestData(personaData);
        }
    }
}
