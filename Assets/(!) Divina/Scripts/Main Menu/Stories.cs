using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stories : MonoBehaviour
{
    //BUG: Clicking too fast will bug out the chapters.

    [Header("Tutorial")]
    [SerializeField] private Button startTutorial;

    [Header("Chapter 1")]
    [SerializeField] private Button startChapterOne;

    [Header("Chapter 2")]
    [SerializeField] private Button startChapterTwo;

    [Header("Chapter 3")]
    [SerializeField] private Button startChapterThree;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTutorial.onClick.AddListener(() => 
        {
            SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
        });
        startChapterOne.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Chapter 1", LoadSceneMode.Single);
        });
    }

    // Update is called once per frame
    void Update()
    {
        forceProgress();
    }

    void forceProgress()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {

        }
    }
}
