using UnityEngine;
using UnityEngine.UI;

public class MessagesScript : MonoBehaviour
{
    [Header("Message Log")]
    [SerializeField] private Button Person1Enter;
    [SerializeField] private Button Back;
    [SerializeField] private GameObject Logs;
    [SerializeField] private GameObject Person1Chat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Person1Chat.SetActive(false);

        Person1Enter.onClick.AddListener(() =>
        {
            Person1Chat.SetActive(true);
        });

        Back.onClick.AddListener(() =>
        {
            Person1Chat.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
