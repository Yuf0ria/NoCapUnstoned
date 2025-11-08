using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("Chapter 1", LoadSceneMode.Single);
    }
}
