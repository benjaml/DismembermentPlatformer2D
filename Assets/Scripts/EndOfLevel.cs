using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EndOfLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	
	// Update is called once per frame
	void Update () {
	    
	}
 
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.transform.parent.GetComponent<PlayerManager>())
        {
            if(collider.transform.parent.GetComponent<PlayerManager>().currentState == PlayerManager.state.FullBody)
                GameManager.instance.LoadLevel(GameManager.instance.GetLevel() + 1);

        }
    }
}
