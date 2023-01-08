using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DrivingSimulation
{
    public class UI_ChallengePersonaGroup : MonoBehaviour
    {
        [SerializeField] private UI_ChallengeData _challengeUI;
        [SerializeField] private UI_PersonaData _personaUI;
        [SerializeField] private TMP_Text _numberSet;

        public void Initialize(ChallengeGeneratedData challengeData, PersonaDataset personaData, bool numberSetActive = false, int number = 0)
        {
            _numberSet.text = $"SET {number}";
            _numberSet.gameObject.SetActive(numberSetActive);

            _challengeUI.Initialize(challengeData);
            _personaUI.Initialize(personaData);
        }
    }
}
