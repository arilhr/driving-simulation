using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    [Serializable]
    public class PersonaDataset
    {
        // Stop Sign Obstacle
        public float WrongStopSign = 0;
        public float CorrectStopSign = 0;

        // Traffic Light Obstacle
        public float WrongTrafficLight = 0;
        public float CorrectTrafficLight = 0;

        // Speed Limit
        public float PassMaxSpeed = 0;
        public float ViolateMaxSpeed = 0;
        //public float PassMinSpeed = 0;
        //public float ViolateMinSpeed = 0;

        // Lane
        public float WrongLane = 0;

        // Crash
        public float Crash = 0;

        // Indicator
        public float WrongIndicator = 0;
        public float CorrectIndicator = 0;

        public PersonaDataset() { }
        public PersonaDataset(PersonaDataset value)
        {
            WrongStopSign = value.WrongStopSign;
            CorrectStopSign = value.CorrectStopSign;
            WrongTrafficLight = value.WrongTrafficLight;
            CorrectTrafficLight = value.CorrectTrafficLight;
            PassMaxSpeed = value.PassMaxSpeed;
            ViolateMaxSpeed = value.ViolateMaxSpeed;
            WrongLane = value.WrongLane;
            Crash = value.Crash;
            WrongIndicator = value.WrongIndicator;
            CorrectIndicator = value.CorrectIndicator;
        }

        public void Average(PersonaDataset p2)
        {
            WrongStopSign = (float)Math.Round(((WrongStopSign + p2.WrongStopSign) / 2), 2);
            CorrectStopSign = (float)Math.Round(((CorrectStopSign + p2.CorrectStopSign) / 2), 2);
            WrongTrafficLight = (float)Math.Round(((WrongTrafficLight + p2.WrongTrafficLight) / 2), 2);
            CorrectTrafficLight = (float)Math.Round(((CorrectTrafficLight + p2.CorrectTrafficLight) / 2), 2);
            PassMaxSpeed = (float)Math.Round(((PassMaxSpeed + p2.PassMaxSpeed) / 2), 2);
            ViolateMaxSpeed = (float)Math.Round(((ViolateMaxSpeed + p2.ViolateMaxSpeed) / 2), 2);
            WrongLane = (float)Math.Round(((WrongLane + p2.WrongLane) / 2), 2);
            Crash = (float)Math.Round(((Crash + p2.Crash) / 2), 2);
            WrongIndicator = (float)Math.Round(((WrongIndicator + p2.WrongIndicator) / 2), 2);
        }

    }
}
