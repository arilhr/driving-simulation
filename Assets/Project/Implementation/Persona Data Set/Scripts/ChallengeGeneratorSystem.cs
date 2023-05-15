using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DrivingSimulation
{
    [Serializable]
    public class ChallengeGeneratedData
    {
        public float LongRoad;
        public float RoadSize;
        public float TotalTurn;
        public float TotalIntersect;
        public float TotalStopSign;
        public float TotalTrafficLight;
        public float TotalMaxSpeedLimit;

        public ChallengeGeneratedData() { }
        public ChallengeGeneratedData(ChallengeGeneratedData value)
        {
            LongRoad = value.LongRoad;
            RoadSize = value.RoadSize;
            TotalTurn = value.TotalTurn;
            TotalIntersect = value.TotalIntersect;
            TotalStopSign = value.TotalStopSign;
            TotalTrafficLight = value.TotalTrafficLight;
            TotalMaxSpeedLimit = value.TotalMaxSpeedLimit;
        }

        public void Average(ChallengeGeneratedData data2)
        {
            LongRoad = (float)Math.Round(((LongRoad + data2.LongRoad) / 2), 2);
            RoadSize = (float)Math.Round(((RoadSize + data2.RoadSize) / 2), 2);
            TotalTurn = (float)Math.Round(((TotalTurn + data2.TotalTurn) / 2), 2);
            TotalIntersect = (float)Math.Round(((TotalIntersect + data2.TotalIntersect) / 2), 2);
            TotalStopSign = (float)Math.Round(((TotalStopSign + data2.TotalStopSign) / 2), 2);
            TotalTrafficLight = (float)Math.Round(((TotalTrafficLight + data2.TotalTrafficLight) / 2), 2);
            TotalMaxSpeedLimit = (float)Math.Round(((TotalMaxSpeedLimit + data2.TotalMaxSpeedLimit) / 2), 2);
        }
    }

    public class ChallengeGeneratorSystem
    {
        private static readonly int[] WRONG_LANE_POINT_CLASSIFICATION = { 0, 3, 5 };
        private static readonly int[] ROAD_SIZE_CLASSIFICATION = { 20, 30, 40 }; // meter
        private static readonly int[] LONG_ROAD_CLASSIFICATION = { 700, 1000, 1500, 2000 }; // meter
        private static readonly float TURN_PER_DISTANCE_MIN = 0;
        private static readonly float TURN_PER_DISTANCE_MAX = 300;
        private static readonly float[] TOTAL_TURN_CLASSIFICATION = { 0, 5, 10, 15 }; // percentage from long road
        private static readonly int[] TOTAL_INTERSECTION_CLASSIFICATION = { 0, 1, 2, 4 };

        public static ChallengeGeneratedData GenerateChallenge(ChallengeGeneratedData challengeData, PersonaDataset personaData)
        {
            ChallengeGeneratedData challengeGeneratedData = new();

            // latest challenge condition
            bool isLatestChallengeHasStopSign = challengeData.TotalStopSign != 0;
            bool isLatestChallengeHasTrafficLight = challengeData.TotalTrafficLight != 0;
            bool isLatestChallengeHasIntersect = challengeData.TotalIntersect == 0;
            bool isLatestChallengeHasSpeedLimit = challengeData.TotalMaxSpeedLimit == 0;
            bool isLatestChallengeHasLongerRoad = challengeData.LongRoad > 1500;

            // DEFINE ROAD SIZE AND LONG ROAD BY WRONG LANE AND CRASH FROM PERSONA DATA
            int wrongLaneClassification = 0;
            for (int i = 1; i < WRONG_LANE_POINT_CLASSIFICATION.Length; i++)
            {
                if (personaData.WrongLane <= WRONG_LANE_POINT_CLASSIFICATION[i]) break;

                wrongLaneClassification = i;
            }

            challengeGeneratedData.RoadSize = ROAD_SIZE_CLASSIFICATION[wrongLaneClassification];
            challengeGeneratedData.LongRoad = Random.Range(LONG_ROAD_CLASSIFICATION[wrongLaneClassification + 1], LONG_ROAD_CLASSIFICATION[wrongLaneClassification]);

            // set turn lane perentage
            int currentTurnLanePercentage = Mathf.RoundToInt(Random.Range(TOTAL_TURN_CLASSIFICATION[wrongLaneClassification + 1], TOTAL_TURN_CLASSIFICATION[wrongLaneClassification]));
            float distancePerTurn = Random.Range(TURN_PER_DISTANCE_MIN, TURN_PER_DISTANCE_MAX);
            int possibleTurnCount = Mathf.RoundToInt(challengeGeneratedData.LongRoad / distancePerTurn);
            challengeGeneratedData.TotalTurn = Mathf.RoundToInt(currentTurnLanePercentage / 100f * possibleTurnCount);

            // DEFINE INTERSECTION AND THE OBSTACLE (STOP SIGN, TRAFFIC LIGHT)

            // get past challenge data intersection classification
            int pastIntersectionClassification = 0;
            for (int i = 0; i < TOTAL_INTERSECTION_CLASSIFICATION.Length - 1; i++)
            {
                if (challengeData.TotalIntersect <= TOTAL_INTERSECTION_CLASSIFICATION[i]) break;

                pastIntersectionClassification = i;
            }

            // get percentage of all point that affected with intersection (stop sign, traffic light, indicator)
            float stopSignWrongPercentage = personaData.WrongStopSign / (personaData.WrongStopSign + personaData.CorrectStopSign);
            float trafficLightPercentage = personaData.WrongTrafficLight / (personaData.WrongTrafficLight + personaData.CorrectTrafficLight);
            float indicatorWrongPercentage = personaData.WrongIndicator / (personaData.WrongIndicator + personaData.CorrectIndicator);

            // define many intersection that will be created in this challenge by the long road, past many intersection challenge data, and wrong obstacle percentage from player
            float totalIntersectionErrorPercentage = (stopSignWrongPercentage + trafficLightPercentage + indicatorWrongPercentage) / 3;

            int currentIntersectionClassification;
            if (totalIntersectionErrorPercentage >= 0.5)
            {
                currentIntersectionClassification = Mathf.Clamp(pastIntersectionClassification + 2, 0, TOTAL_INTERSECTION_CLASSIFICATION.Length - 2);
            }
            else if (totalIntersectionErrorPercentage >= 0.3)
            {
                currentIntersectionClassification = pastIntersectionClassification;
            }
            else
            {
                currentIntersectionClassification = Mathf.Clamp(pastIntersectionClassification - 2, 0, TOTAL_INTERSECTION_CLASSIFICATION.Length - 2);
            }

            challengeGeneratedData.TotalIntersect = Random.Range(TOTAL_INTERSECTION_CLASSIFICATION[currentIntersectionClassification + 1], TOTAL_INTERSECTION_CLASSIFICATION[currentIntersectionClassification]);

            // define stop sign will spawn in this challenge or not
            bool isSpawningStopSign = !isLatestChallengeHasStopSign || stopSignWrongPercentage > 0;
            bool isSpawningTrafficLight = !isLatestChallengeHasTrafficLight || trafficLightPercentage > 0;

            if (isSpawningStopSign && isSpawningTrafficLight)
            {
                float stopPerTrafficPercentage = stopSignWrongPercentage / (stopSignWrongPercentage + trafficLightPercentage);

                challengeGeneratedData.TotalStopSign = Mathf.RoundToInt(stopPerTrafficPercentage * challengeGeneratedData.TotalIntersect);
                challengeGeneratedData.TotalTrafficLight = Mathf.RoundToInt((1 - stopPerTrafficPercentage) * challengeGeneratedData.TotalIntersect);

            }
            else if (isSpawningStopSign)
            {
                challengeGeneratedData.TotalStopSign = challengeGeneratedData.TotalIntersect;
            }
            else if (isSpawningTrafficLight)
            {
                challengeGeneratedData.TotalTrafficLight = challengeGeneratedData.TotalIntersect;
            }

            return challengeGeneratedData;
        }
    }
}
