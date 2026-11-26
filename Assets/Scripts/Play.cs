using UnityEngine;
using UnityEngine.SceneManagement;
public class Play : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Gameplay");
    }
}
