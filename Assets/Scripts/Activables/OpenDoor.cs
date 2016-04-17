using UnityEngine;
using System.Collections;

public class OpenDoor : AbstractActivable {

    public override void Activate()
    {
        GetComponent<Animator>().SetTrigger("open");
    }

}
