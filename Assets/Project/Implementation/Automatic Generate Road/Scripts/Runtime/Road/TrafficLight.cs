using DrivingSimulation;
using UnityEngine;

public enum TrafficLightMode
{
    Red, 
    Yellow,
    Green
}

public class TrafficLight : MonoBehaviour
{
    [Header("3D Object")]
    public GameObject verticalPole;
    public GameObject horizontalPole;
    public GameObject lightObj;

    [Header("References")]
    [SerializeField] private MeshRenderer lightRenderer;

    private const string PLAYER_LAYER_NAME = "Player";
    private const string CROSSING_WRONG_LIGHT_MESSAGE = "Crossing Wrong Light!";
    private bool isGreen = false;

    // Persona
    private const string PERSONA_KEY = "wrongTrafficLight";

    public void Initialize(float scale)
    {
        verticalPole.transform.localScale *= scale;
        horizontalPole.transform.localScale = new Vector3(
            horizontalPole.transform.localScale.x * scale, 
            horizontalPole.transform.localScale.y * scale, 
            1f);
        horizontalPole.transform.localPosition = new Vector3(0, horizontalPole.transform.localPosition.y * scale, 0);
        lightObj.transform.localScale *= scale;
        lightObj.transform.localPosition = new Vector3(
            lightObj.transform.localPosition.x,
            lightObj.transform.localPosition.y * scale,
            lightObj.transform.localPosition.z);
    }

    public void ChangeLight(TrafficLightMode mode)
    {
        if (mode == TrafficLightMode.Red)
        {
            lightRenderer.material.mainTextureOffset = new Vector2(0f, 0f);
            isGreen = false;
            return;
        }

        if (mode == TrafficLightMode.Yellow)
        {
            lightRenderer.material.mainTextureOffset = new Vector2(0.334f, 0f);
            isGreen = false;
            return;
        }

        if (mode == TrafficLightMode.Green)
        {
            lightRenderer.material.mainTextureOffset = new Vector2(0.667f, 0f);
            isGreen = true;
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer(PLAYER_LAYER_NAME)) return;

        Vector3 forwardDir = 1f * transform.forward;

        Vector3 directionToOther = other.transform.root.position - transform.position;

        float angle = Vector3.Angle(forwardDir, directionToOther);

        if (angle < 90f)
        {
            if (isGreen)
            {
                Success();
                return;
            }

            Failed();
        }
    }

    private void Success()
    {
        if (GlobalEvents.Instance != null)
        {
            GlobalEvents.Instance.SetNotificationCallback.Invoke("Green Light Crossing!", (int)NotificationType.Success);
            GlobalEvents.Instance.StartNoticationCallback.Invoke(1f, 3f, 1f);

            GlobalEvents.Instance.AddPointCallback.Invoke(20);
        }
    }

    private void Failed()
    {
        if (GlobalEvents.Instance != null)
        {
            GlobalEvents.Instance.SetNotificationCallback.Invoke("You crossing the red light!", (int)NotificationType.Danger);
            GlobalEvents.Instance.StartNoticationCallback.Invoke(1f, 3f, 1f);

            GlobalEvents.Instance.AddPointCallback.Invoke(-20);
            GlobalEvents.Instance.AddMistakeCallback.Invoke(CROSSING_WRONG_LIGHT_MESSAGE, 1);
        }

        if (PersonaDataTracker.Instance != null)
        {
            PersonaDataTracker.Instance.Add(PERSONA_KEY);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 0.5f);
    }
#endif
}
