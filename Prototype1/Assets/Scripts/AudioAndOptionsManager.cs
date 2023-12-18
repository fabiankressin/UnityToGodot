using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioAndOptionsManager : MonoBehaviour
{
    public static AudioAndOptionsManager Instance { get; private set; }
    public float masterVolume = 0.5f;
    public string currentScene;
    public string previousScene;
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("AudioAndOptionsManager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);

        if (AudioAndOptionsManager.Instance == null)
        {
            Instance = this;
            currentScene = SceneManager.GetActiveScene().name;
            AudioListener.volume = masterVolume;
        }
    }

    public void LoadScene(string sceneName)
    {
        previousScene = currentScene;
        currentScene = sceneName;
        SceneManager.LoadScene(sceneName);
        if (GameObject.FindObjectOfType<AudioListener>() == null)
        {
            this.gameObject.AddComponent<AudioListener>();
        }
        else if (GameObject.FindObjectsOfType<AudioListener>().Length > 1 && this.gameObject.GetComponent<AudioListener>() != null)
        {
            Destroy(this.gameObject.GetComponent<AudioListener>());
        }
        AudioListener.volume = masterVolume;
    }

    public string getPreviousScene()
    {
        return previousScene;
    }

    public void setMasterVolume(float volume)
    {
        masterVolume = volume;
        AudioListener.volume = masterVolume;
    }

    public float getMasterVolume()
    {
        return masterVolume;
    }
}
