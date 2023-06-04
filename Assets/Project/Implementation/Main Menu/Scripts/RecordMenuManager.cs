using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class RecordMenuManager : MonoBehaviour
    {
        [BoxGroup("UI")]
        [SerializeField] private Transform contentParent = null;
        [BoxGroup("UI")]
        [SerializeField] private UI_MistakeItem contentPrefabs = null;

        private Dictionary<string, UI_MistakeItem> objs = new Dictionary<string, UI_MistakeItem>();

        private void OnEnable()
        {
            PersonaData personaData = new PersonaData();

            if (ChallengeDataManager.Instance != null)
            {
                personaData = ChallengeDataManager.Instance.AveragePersonaData();
            }

            Initialize(personaData);
        }


        private void Initialize(PersonaData persona)
        {
            // Wrong Lane
            InstantiatePersonaDataUI("Wrong Lane", (int)persona.WrongLane);

            // Crash
            InstantiatePersonaDataUI("Crash", (int)persona.Crash);

            // Wrong Indicator
            InstantiatePersonaDataUI("Wrong Indicator", (int)persona.WrongIndicator);

            // Violate Speed Limit
            InstantiatePersonaDataUI("Violate Speed Limit", (int)persona.ViolateSpeedLimit);
        }

        private void InstantiatePersonaDataUI(string key, int persona)
        {
            if (!objs.TryGetValue(key, out UI_MistakeItem item))
            {
                item = Instantiate(contentPrefabs, contentParent);
                objs[key] = item;
            }

            item.SetText(key, persona);
        }
    }
}
