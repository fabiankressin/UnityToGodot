using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsUI : MonoBehaviour
{

    [SerializeField] public Button saveAndQuitButton;
    [SerializeField] public Slider masterVolumeSlider;
    [SerializeField] public TextMeshProUGUI masterVolumeText;
    private float masterVolume;

    private void Awake()
    {
        masterVolume = AudioAndOptionsManager.Instance.getMasterVolume();

        saveAndQuitButton.onClick.AddListener(() =>
        {
            AudioAndOptionsManager.Instance.setMasterVolume(masterVolume);
            AudioAndOptionsManager.Instance.LoadScene(AudioAndOptionsManager.Instance.getPreviousScene());
        });
        masterVolumeSlider.onValueChanged.AddListener((float value) =>
        {
            masterVolume = Mathf.Round(value * 100.0f) * 0.01f;
            masterVolumeText.text = Mathf.Round(value * 100.0f).ToString();
        });

        masterVolumeSlider.value = masterVolume;
    }
}
