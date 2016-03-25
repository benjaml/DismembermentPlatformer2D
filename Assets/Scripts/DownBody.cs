using UnityEngine;
using System.Collections;

public class DownBody : MonoBehaviour {

    public float inertie;
    public bool moveAlone = false;
    public bool isGrounded = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(moveAlone)
            transform.position += new Vector3(inertie*Time.deltaTime, 0.0f, 0.0f);
	}

    public void SetInertie(float speed)
    {
        inertie = speed;
        moveAlone = true;
    }
    public void StopMovement()
    {
        inertie = 0;
        moveAlone = false;
    }
}
