using UnityEngine;
using System.Collections;

public class UpBody : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "down")
        {
            transform.parent.GetComponent<PlayerManager>().resetFullBody();
        }
    }
}
