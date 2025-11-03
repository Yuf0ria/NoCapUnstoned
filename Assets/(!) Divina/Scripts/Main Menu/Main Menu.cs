using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button contButton;
    [SerializeField] private Button storiesButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //startButton.onClick.AddListener(startGame);
        //exitButton.onClick.AddListener(exitGame);

        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Tutorial", LoadSceneMode.Additive);
        });

        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
