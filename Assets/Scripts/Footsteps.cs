using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Footsteps : MonoBehaviour
{
    public AudioSource FootstepsSound;
    void update()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            FootstepsSound.enabled = true;
        }
        else
        {
            FootstepsSound.enabled = false;
        }
    }
}
