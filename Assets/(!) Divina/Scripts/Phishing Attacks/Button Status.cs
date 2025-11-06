using UnityEngine;
using UnityEngine.UI;

public class ButtonStatus : MonoBehaviour
{
    [SerializeField] private string Status;

    private Button hyperlink;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hyperlink = GetComponent<Button>();
        hyperlink.onClick.AddListener(delegate { Debug.Log(Status); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
