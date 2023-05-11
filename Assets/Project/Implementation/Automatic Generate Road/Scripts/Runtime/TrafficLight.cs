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
    private bool isGreen = false;

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
            return;
        }

        if (mode == TrafficLightMode.Yellow)
        {
            lightRenderer.material.mainTextureOffset = new Vector2(0.334f, 0f);
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

        Vector3 rotation = transform.localRotation.eulerAngles;
        Quaternion offset = Quaternion.Euler(rotation.x, rotation.y + 90f, rotation.z);
        Vector3 forwardDir = offset * transform.forward;

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
        Debug.Log("<color=green>Success pass traffic light.</color>");
    }

    private void Failed()
    {
        Debug.Log("<color=red>Wrong traffic light.</color>");
    }

}
