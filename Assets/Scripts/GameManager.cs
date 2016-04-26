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
            LoadLevel(3);
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Debug.Log(GetLevel());
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
        Debug.Log("Load level " + number);
        SceneManager.LoadScene(number);
        SetLevel(number);
    }
}
