using UnityEngine;

public class Collision : MonoBehaviour
{
    void OnCollisionStay(Collision c)
    {
        Debug.Log("Standing on: " + c.gameObject.name, c.gameObject);
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Standing on: " + hit.gameObject.name, hit.gameObject);
    }
}
