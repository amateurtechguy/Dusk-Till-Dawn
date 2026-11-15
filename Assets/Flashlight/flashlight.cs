using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    public GameObject ON;
    public GameObject OFF;
    public AudioSource audioSource;
    private bool isON;

    void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }
    }

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
            Debug.LogWarning("AudioSource not found");
            return;
        }
        audioSource.Play();
    }
}