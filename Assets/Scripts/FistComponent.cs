using UnityEngine;
using System.Collections;
using System;

public class FistComponent : MonoBehaviour {

    public float velocityX;
    public float velocityY;
    public float force;
    public float gravity;
    public float friction;
    private bool fired = false;
    
	
	// Update is called once per frame
	void Update () {
        if (!fired)
            return;
        velocityY -= friction * Time.deltaTime;
        velocityX = Mathf.Max(0.0f, velocityX);
        velocityY -= gravity * Time.deltaTime;
        transform.position += new Vector3(velocityX, velocityY, 0.0f) * Time.deltaTime;
	}

    internal void Fire(Vector3 direction)
    {
        direction *= force;
        velocityX = direction.x;
        velocityY = direction.y;
        fired = true;
    }
}
