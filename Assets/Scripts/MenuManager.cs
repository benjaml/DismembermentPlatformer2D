using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Return) && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
        {
            GameManager.instance.LoadLevel(6);
        }
        if (Input.GetKeyDown(KeyCode.Return) && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial"))
        {
            GameManager.instance.LoadLevel(2);
        }
	}
}
