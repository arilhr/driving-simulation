using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class UI_MistakeManager : MonoBehaviour
    {
        public Transform ContentContainer = null;
        public UI_MistakeItem UIMistakeItem = null;

        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize()
        {
            ClearItem();

            if (PlayerMistakeAnalytics.Instance == null) return;

            Dictionary<string, int> mistakes = PlayerMistakeAnalytics.Instance.Mistakes;

            if (mistakes.Count == 0) return;

            foreach (KeyValuePair<string, int> kvp in mistakes)
            {
                SpawnItem(kvp.Key, kvp.Value);
            }
        }

        private void SpawnItem(string key, int amount)
        {
            GameObject item = Instantiate(UIMistakeItem.gameObject, ContentContainer);
            UI_MistakeItem mistakeItem = item.GetComponent<UI_MistakeItem>();

            mistakeItem.SetText(key, amount);
        }

        private void ClearItem()
        {
            for (int i = 0; i < ContentContainer.childCount; i++)
            {
                GameObject child = ContentContainer.GetChild(i).gameObject;
                Destroy(child);
            }
        }
    }
}
