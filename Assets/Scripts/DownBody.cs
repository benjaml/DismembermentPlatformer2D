using UnityEngine;
using System.Collections;

public class DownBody : MonoBehaviour {

    public float inertie;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(inertie, 0.0f, 0.0f);
	}

    public void SetInertie(float speed)
    {
        inertie = speed;
    }
}
