﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EndOfLevel : MonoBehaviour {

    public int levelToLoad;
 
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.parent != null && collider.transform.parent.GetComponent<PlayerManager>())
        {
            if(collider.transform.parent.GetComponent<PlayerManager>().currentState == PlayerManager.state.FullBody)
                GameManager.instance.LoadLevel(levelToLoad);

        }
    }
}
