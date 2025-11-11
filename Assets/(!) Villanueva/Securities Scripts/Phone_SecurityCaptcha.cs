using UnityEngine;
using UnityEngine.UI;

public class Phone_SecurityCaptcha : MonoBehaviour
{
    [Header("Button Prefab + Parent")]
    public GameObject buttonPrefab;
    public RectTransform parentPanel; 

    [Header("Spawn Settings")]
    [SerializeField] private float xMin = -300f;
    [SerializeField] private float xMax = 300f;
    [SerializeField] private float[] yPositions = new float[] { 200f, 0f, -200f };
    [SerializeField] private float speed = 400f;
    [SerializeField] private float speedMax = 600f;

    [Header("Spawn Type")]
    [SerializeField] private bool Easy;
    [SerializeField] private bool Medium;
    [SerializeField] private bool Hard;


    void Start()
    {
        if (Easy) Spawn_EasyCaptcha();
        else if (Medium) Spawn_MediumCaptcha();
        else if (Hard) Spawn_HardCaptcha();
        else this.gameObject.SetActive(false);
    }

    void Spawn_EasyCaptcha()
    {
        if (buttonPrefab == null || parentPanel == null)
        {
            Debug.LogWarning("Missing buttonPrefab or parentPanel reference!");
            return;
        }

        for (int i = 0; i < yPositions.Length; i++)
        {
            // Spawn a copy of the prefab as a child of the parent
            GameObject newButton = Instantiate(buttonPrefab, parentPanel);
            RectTransform rect = newButton.GetComponent<RectTransform>();


            float randomX = Random.Range(xMin, xMax);
            float y = yPositions[i];

            rect.anchoredPosition = new Vector2(randomX, y);
            rect.localScale = Vector3.one;

            Button button = newButton.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                button.interactable = false;
            });
        }
    }

    void Spawn_MediumCaptcha()
    {
        if (buttonPrefab == null || parentPanel == null)
        {
            Debug.LogWarning("Missing buttonPrefab or parentPanel reference!");
            return;
        }

        foreach (float y in yPositions)
        {
            GameObject newButton = Instantiate(buttonPrefab, parentPanel);
            RectTransform rect = newButton.GetComponent<RectTransform>();

            float randomStartX = Random.Range(xMin, xMax);
            rect.anchoredPosition = new Vector2(randomStartX, y);
            rect.localScale = Vector3.one;

            ButtonMover mover = newButton.AddComponent<ButtonMover>();
            mover.Init(xMin, xMax, speed);

            Button button = newButton.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                button.interactable = false;
                mover.enabled = false; // Stop Movement
            });
        }
    }

    void Spawn_HardCaptcha()
    {
        if (buttonPrefab == null || parentPanel == null)
        {
            Debug.LogWarning("Missing buttonPrefab or parentPanel reference!");
            return;
        }

        for (int i = 0; i < yPositions.Length; i++)
        {
            // Spawn a copy of the prefab as a child of the parent
            GameObject newButton = Instantiate(buttonPrefab, parentPanel);
            RectTransform rect = newButton.GetComponent<RectTransform>();


            // Random start position within panel
            float halfWidth = parentPanel.rect.width / 2f;
            float halfHeight = parentPanel.rect.height / 2f;
            Vector2 startPos = new Vector2(
                Random.Range(-halfWidth + 50f, halfWidth - 50f),
                Random.Range(-halfHeight + 50f, halfHeight - 50f)
            );

            rect.anchoredPosition = startPos;
            rect.localScale = Vector3.one;

            // Add bouncing mover
            ButtonBouncer mover = newButton.AddComponent<ButtonBouncer>();
            mover.Init(parentPanel, speed);

            // Disable on click
            Button btn = newButton.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                btn.interactable = false;
                mover.enabled = false;
            });
        }
    }
}
