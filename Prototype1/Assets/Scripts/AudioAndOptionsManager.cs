using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioAndOptionsManager : MonoBehaviour
{
    public static AudioAndOptionsManager Instance { get; private set; }
    public float masterVolume = 1f;
    public string currentScene;
    public string previousScene;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("AudioAndOptionsManager Awake");
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
        }
    }

    public void LoadScene(string sceneName)
    {
        previousScene = currentScene;
        currentScene = sceneName;
        SceneManager.LoadScene(sceneName);
    }

    public string getPreviousScene()
    {
        return previousScene;
    }

    public void setMasterVolume(float volume)
    {
        masterVolume = volume;
    }

    public float getMasterVolume()
    {
        return masterVolume;
    }
}
