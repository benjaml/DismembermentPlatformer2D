using UnityEngine;
using System.Collections;
using System;

public class CloseDoor : AbstractActivable  {

    public override void Activate()
    {
        GetComponent<Animator>().SetTrigger("close");
    }
    
}
