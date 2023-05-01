using UnityEngine;

public enum TrafficLightMode
{
    Red, 
    Yellow,
    Green
}

public class TrafficLight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MeshRenderer lightRenderer;

    private string PLAYER_LAYER_NAME = "Player";
    private bool isGreen = false;

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
