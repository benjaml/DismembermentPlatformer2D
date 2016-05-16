using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Invoke("SplashScreen", 1f);
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    void SplashScreen()
    {
        LoadLevel(1);
    }

    public void SetLevel(int number)
    {
        PlayerPrefs.SetInt("level", number);

    }

    public int GetLevel()
    {
        return PlayerPrefs.GetInt("level");
    }

    public void Reload()
    {
        SceneManager.LoadScene(GetLevel());
    }

    public void LoadLevel(int number)
    {
        SceneManager.LoadScene(number);
        SetLevel(number);
    }
}
