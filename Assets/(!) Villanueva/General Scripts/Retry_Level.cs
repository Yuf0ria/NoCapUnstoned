using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry_Level : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Retry()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
