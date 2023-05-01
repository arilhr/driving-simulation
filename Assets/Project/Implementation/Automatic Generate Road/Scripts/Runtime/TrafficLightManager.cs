using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightManager : MonoBehaviour
{
    public float interval;
    public List<TrafficLight> trafficLights = new List<TrafficLight>();
    private int currentGreenLight = 0;
    private bool isRun = false;

    private void Start()
    {
        Debug.Log($"{interval} | {trafficLights.Count}");

        if (interval > 0 && trafficLights.Count > 0)
        {
            StartCoroutine(StartCount());
        }
    }

    public void Initialize(float interval, List<TrafficLight> trafficLights)
    {
        this.interval = interval;
        this.trafficLights = trafficLights;
    }

    private IEnumerator StartCount()
    {
        Debug.Log($"Start Traffic Light: {interval}");
        isRun = true;

        while (isRun)
        {
            // turn green current traffic light
            trafficLights[currentGreenLight].ChangeLight(TrafficLightMode.Green);

            yield return new WaitForSeconds(interval);

            // turn red previous traffic ligth
            trafficLights[currentGreenLight].ChangeLight(TrafficLightMode.Red);

            // get next traffic light
            currentGreenLight++;
            if (currentGreenLight >= trafficLights.Count)
            {
                currentGreenLight = 0;
            }
        }
    }

    public void Stop()
    {
        isRun = false;
    }
}