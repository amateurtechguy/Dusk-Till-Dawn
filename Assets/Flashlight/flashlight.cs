using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    public GameObject ON;
    public GameObject OFF;
    public AudioSource audioSource;
    public AudioClip clickSound;
    private bool isON;

    void Start()
    {
        isON = false;
        UpdateState();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            isON = !isON;
            UpdateState();
            PlayClick();
        }
    }

    void UpdateState()
    {
        ON.SetActive(isON);
        OFF.SetActive(!isON);
    }

    void PlayClick()
{
    if (audioSource == null)
    {
        Debug.LogWarning("AudioSource is not assigned");
        return;
    }
    if (clickSound == null)
    {
        Debug.LogWarning("ClickSound is not assigned");
        return;
    }
    Debug.Log("Playing click");
    audioSource.PlayOneShot(clickSound);
}
}