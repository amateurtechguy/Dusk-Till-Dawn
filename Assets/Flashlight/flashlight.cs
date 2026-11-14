using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject ON;
    public GameObject OFF;
    private bool isON;
    void Start()
    {
        ON.SetActive(false);
        OFF.SetActive(true);
        isON = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
           
        if (isON)
        {
            ON.SetActive(false);
            OFF.SetActive(true);
        } 
        if (!isON)
        {
            ON.SetActive(true);
            OFF.SetActive(false);
        } 
        isON = !isON;
        }
    }
}
