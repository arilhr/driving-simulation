using Sirenix.OdinInspector;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DrivingSimulation
{
    public class SceneChanger : SingletonDontDestroy<SceneChanger>
    {
        [BoxGroup("Loading UI")]
        [SerializeField] private GameObject loadingCanvas;
        [BoxGroup("Loading UI")]
        [SerializeField] private Image loadingBar;
        [BoxGroup("Loading UI")]
        [SerializeField] private TMP_Text loadingText;

        public async void LoadScene(string sceneName)
        {
            var scene = SceneManager.LoadSceneAsync(sceneName);
            scene.allowSceneActivation = false;
            UpdateLoadingBar(0);
            UpdateLoadingText("0%");

            loadingCanvas.SetActive(true);

            // checking if scene already loaded
            do
            {
                await Task.Delay(100);

                UpdateLoadingBar(scene.progress);
                UpdateLoadingText($"{Math.Round(scene.progress * 100)}%");

            } while (scene.progress < 0.9f);

            await Task.Delay(1000);

            scene.allowSceneActivation = true;
            UpdateLoadingBar(1);
            UpdateLoadingText($"100%");

            await Task.Delay(500);

            loadingCanvas.SetActive(false);
        }

        public void UpdateLoadingBar(float amount)
        {
            if (loadingBar == null)
                return;

            loadingBar.fillAmount = amount;
        }

        public void UpdateLoadingText(string text)
        {
            if (loadingText == null) 
                return;

            loadingText.text = text;
        }
    }
}
