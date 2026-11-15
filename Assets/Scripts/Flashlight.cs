using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class Flashlight : MonoBehaviour
{
    public GameObject ON;
    public GameObject OFF;
    public AudioClip clickSound;
    [Range(0f, 1f)] public float volume = 1f;
    private AudioSource audioSource;
    private bool isON;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
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
        if (clickSound == null)
        {
            Debug.LogWarning("Click sound not assigned");
            return;
        }
        audioSource.PlayOneShot(clickSound, volume);
    }
}