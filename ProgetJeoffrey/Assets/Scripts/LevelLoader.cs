using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] Slider slider = null;
    [SerializeField] TextMeshProUGUI progressText = null;

    private void Start()
    {
        LevelManager.Instance.InitLevelLoader(this);
        gameObject.SetActive(false);
    }

    internal void SetProgress(float progress)
    {
        slider.value = progress;
        progressText.text = progress * 100 + "%";
    }
}
