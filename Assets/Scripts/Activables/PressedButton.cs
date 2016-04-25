using UnityEngine;
using System.Collections;
using System;

public class PressedButton : AbstractActivable
{
    public override void Activate()
    {
        GetComponent<Animator>().SetTrigger("press");
    }
}
