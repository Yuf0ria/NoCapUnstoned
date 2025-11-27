using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void exitScene()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
