using UnityEngine;
using UnityEngine.InputSystem;

public class Footsteps : MonoBehaviour
{
    public AudioSource FootstepsSound;

    void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        bool moving = kb.wKey.isPressed || kb.aKey.isPressed || kb.sKey.isPressed || kb.dKey.isPressed;

        if (moving && !FootstepsSound.isPlaying)
        {
            FootstepsSound.Play();
        }
        else if (!moving && FootstepsSound.isPlaying)
        {
            FootstepsSound.Stop();
        }
    }
}