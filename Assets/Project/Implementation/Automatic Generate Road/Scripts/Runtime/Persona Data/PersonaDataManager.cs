using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DrivingSimulation
{
    public class PersonaDataValue
    {
        public Dictionary<string, int> Value = new Dictionary<string, int>();
    }

    public class PersonaDataManager : SingletonDontDestroy<PersonaDataManager>
    {
        #region Variables

        private const string PERSONA_DATA_FILE_NAME = "persona-data.json";

        public PersonaDataKey personaDataKey;
        public int maxRecordedData = 5;

        private List<PersonaDataValue> personaDataValueLists = new List<PersonaDataValue>();

        #endregion

        #region Method

        public void AddLatestData(PersonaDataValue newValue)
        {
            personaDataValueLists.Add(newValue);

            if (personaDataValueLists.Count > maxRecordedData)
            {
                personaDataValueLists.RemoveAt(maxRecordedData - 1);
            }
        }

        public PersonaDataValue AveragePersonaData()
        {
            PersonaDataValue avg = new PersonaDataValue();

            foreach (PersonaDataValue value in personaDataValueLists)
            {
                avg = AddTwoPersona(avg, value);
            }

            return avg;
        }

        private void LoadData()
        {

        }

        private void SaveData()
        {

        }

        private PersonaDataValue NewPersonaData()
        {
            PersonaDataValue newPersona = new PersonaDataValue();

            foreach (string key in personaDataKey.Keys)
            {
                newPersona.Value.Add(key, 0);
            }

            return newPersona;
        }

        private PersonaDataValue AddTwoPersona(PersonaDataValue persona1, PersonaDataValue persona2)
        {
            PersonaDataValue result = new PersonaDataValue();

            foreach (string key in personaDataKey.Keys)
            {
                int value1 = 0;
                int value2 = 0;

                if (persona1.Value.ContainsKey(key))
                {
                    value1 = persona1.Value[key];
                }

                if (persona2.Value.ContainsKey(key))
                {
                    value2 = persona2.Value[key];
                }

                result.Value.Add(key, value1 + value2 / 2);
            }

            return result;
        }

        #endregion
    }
}
