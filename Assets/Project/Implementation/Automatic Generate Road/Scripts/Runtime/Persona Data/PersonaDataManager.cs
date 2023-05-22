using System;
using System.Collections.Generic;

namespace DrivingSimulation
{
    public class PersonaDataValue
    {
        public Dictionary<string, int> Value = new Dictionary<string, int>();
    }

    [Serializable]
    public class PersonaData
    {
        public float WrongLane = 0;
        public float Crash = 0;
        public float WrongIndicator = 0;
        public float CorrectIndicator = 0;
        public float WrongStopSign = 0;
        public float WrongTrafficLight = 0;
        public float ViolateSpeedLimit = 0;

        public PersonaData Average(PersonaData persona2)
        {
            PersonaData result = new PersonaData();

            result.WrongLane = (WrongLane + persona2.WrongLane) / 2f;
            result.Crash = (Crash + persona2.Crash) / 2f;
            result.WrongIndicator = (WrongIndicator + persona2.WrongIndicator) / 2f;
            result.CorrectIndicator = (CorrectIndicator + persona2.CorrectIndicator) / 2f;
            result.WrongStopSign = (WrongStopSign + persona2.WrongStopSign) / 2f;
            result.WrongTrafficLight = (WrongTrafficLight + persona2.WrongTrafficLight) / 2f;
            result.ViolateSpeedLimit = (ViolateSpeedLimit + persona2.ViolateSpeedLimit) / 2f;

            return result;
        }
    }

    public class PersonaDataManager : SingletonDontDestroy<PersonaDataManager>
    {
        #region Variables

        private const string PERSONA_DATA_FILE_NAME = "persona-data.json";

        public int maxRecordedData = 5;

        private List<PersonaData> personaDataValueLists = new List<PersonaData>();

        #endregion

        #region Method

        public void AddLatestData(PersonaData newValue)
        {
            personaDataValueLists.Add(newValue);

            if (personaDataValueLists.Count > maxRecordedData)
            {
                personaDataValueLists.RemoveAt(maxRecordedData - 1);
            }
        }

        public PersonaData AverageAll()
        {
            PersonaData avg = new PersonaData();

            for (int i = 0; i < personaDataValueLists.Count; i++)
            {
                if (i == 0)
                {
                    avg = personaDataValueLists[0];
                    continue;
                }

                avg = avg.Average(personaDataValueLists[i]);
            }

            return avg;
        }

        private void LoadData()
        {

        }

        private void SaveData()
        {

        }

        private PersonaData NewPersonaData()
        {
            PersonaData newPersona = new PersonaData();

            return newPersona;
        }

        #endregion
    }
}
