using UnityEngine;

namespace DrivingSimulation
{
    public class RepeatedObject : MonoBehaviour
    {
        public GameObject objPrefabs;
        [Min(1f)]
        public float width;
        [Min(1f)]
        public float space;

        GameObject parentObj;

        void Initialize()
        {
            DestroyChild();

            parentObj = new GameObject("Container");
            parentObj.transform.parent = transform;
            parentObj.transform.localPosition = Vector3.zero;
            parentObj.transform.localRotation = Quaternion.identity;
        }

        public void Build()
        {
            Initialize();

            // spawn center object
            GameObject startObj = Instantiate(objPrefabs, parentObj.transform);
            startObj.transform.localPosition = Vector3.zero;

            float nextPos = space;
            while (nextPos < width / 2f)
            {
                GameObject leftObj = Instantiate(objPrefabs, parentObj.transform);
                leftObj.transform.localPosition = Vector3.left * nextPos;

                GameObject rightObj = Instantiate(objPrefabs, parentObj.transform);
                rightObj.transform.localPosition = Vector3.right * nextPos;

                nextPos += space;
            }
        }

        void DestroyChild()
        {
            // Get all child objects of this game object
            Transform[] children = GetComponentsInChildren<Transform>();

            // Loop through each child and destroy it
            foreach (Transform child in children)
            {
                // Skip the parent object
                if (child == transform) continue;
                if (child == null) continue;

                // Destroy the child object
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
