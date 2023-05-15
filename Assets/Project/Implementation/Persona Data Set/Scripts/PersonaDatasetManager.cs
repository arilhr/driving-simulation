using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DrivingSimulation
{
    [Serializable]
    public class PersonaDatasetSaved
    {
        public List<PersonaDataset> pastGamesPersonaData;
        public PersonaDataset personaDatasetAverage;
    }

    public class PersonaDatasetManager
    {
        private const string SAVED_FILE_NAME = "persona-data.json";

        private static readonly int _maxPastGame = 5;
        private static List<PersonaDataset> _pastGamesPersonaDatasetLists = new();
        private static PersonaDataset _personaDatasetAverage = new();

        public static List<PersonaDataset> PastGamesPersonaDatasetLists
        {
            get { return _pastGamesPersonaDatasetLists; }
        }

        public static void AddNewGamePersona(PersonaDataset persona)
        {
            if (persona == null) return;

            if (_pastGamesPersonaDatasetLists.Count == _maxPastGame)
            {
                _pastGamesPersonaDatasetLists.RemoveAt(_pastGamesPersonaDatasetLists.Count - 1);
            }

            _pastGamesPersonaDatasetLists.Add(persona);

            // Count Average
            _personaDatasetAverage.Average(persona);
        }

        public static void SavePersonaData()
        {
            PersonaDatasetSaved savedData = new();
            savedData.pastGamesPersonaData = _pastGamesPersonaDatasetLists;
            savedData.personaDatasetAverage = _personaDatasetAverage;

            string json = JsonUtility.ToJson(savedData);

            File.WriteAllText(SAVED_FILE_NAME, json);
        }

        public static void LoadPersonaData()
        {
            if (File.Exists(SAVED_FILE_NAME))
            {
                // Load the list from a JSON file
                string json = File.ReadAllText(SAVED_FILE_NAME);
                PersonaDatasetSaved data = JsonUtility.FromJson<PersonaDatasetSaved>(json);

                _pastGamesPersonaDatasetLists = data.pastGamesPersonaData;
                _personaDatasetAverage = data.personaDatasetAverage;

                return;
            }

            Debug.Log($"Persona data is not found!");
        }
    }
}
