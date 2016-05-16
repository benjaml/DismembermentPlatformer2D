using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class debugScript : MonoBehaviour {

    public PlayerManager player;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
        GetComponent<Text>().text = "Player jumpforce " + player.jumpForce + "\nPlayer gravity " + player.gravity + "\nFPS " + 1 / Time.deltaTime + "\nCurrentGravity " + player.gravityVelocity;
	}
}
