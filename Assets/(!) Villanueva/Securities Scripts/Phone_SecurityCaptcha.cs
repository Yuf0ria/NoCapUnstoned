using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Phone_SecurityCaptcha : MonoBehaviour
{
    [Header("Prefab, GameObjects and Position")]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private RectTransform Area_CAPTCHA;
    [SerializeField] private RectTransform Page_CAPTCHA;
    [SerializeField] private Event_Manager events;

    [SerializeField] private Transform openedPos; 
    [SerializeField] private Transform closedPos;
    [SerializeField] private float transitionTime;

    [Header("Button Spawn Settings")]
    [SerializeField] private float xMin = -300f;
    [SerializeField] private float xMax = 300f;
    [SerializeField] private float[] yPositions = new float[] { 200f, 0f, -200f };
    [SerializeField] private float speed = 400f;
    [SerializeField] private float speedMax = 600f;

    [Header("Button Spawn Type")]
    [SerializeField] private bool Easy;
    [SerializeField] private bool Medium;
    [SerializeField] private bool Hard;

    int buttonsPressed = 0;
    List<GameObject> buttons = new List<GameObject>();

    void Start()
    {
        //CAPTCHA_Start();
    }

    void Spawn_EasyCaptcha()
    {
        if (buttonPrefab == null || Area_CAPTCHA == null)
        {
            Debug.LogWarning("Missing buttonPrefab or Area_CAPTCHA reference!");
            return;
        }

        for (int i = 0; i < yPositions.Length; i++)
        {
            // Spawn a copy of the prefab as a child of the parent
            GameObject newButton = Instantiate(buttonPrefab, Area_CAPTCHA);
            buttons.Add(newButton);
            RectTransform rect = newButton.GetComponent<RectTransform>();


            float randomX = Random.Range(xMin, xMax);
            float y = yPositions[i];

            rect.anchoredPosition = new Vector2(randomX, y);
            rect.localScale = Vector3.one;

            Button button = newButton.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                button.interactable = false;
                buttonsPressed++;
                if (buttonsPressed >= 3) CAPTCHA_Success();
            });
        }
    }

    void Spawn_MediumCaptcha()
    {
        if (buttonPrefab == null || Area_CAPTCHA == null)
        {
            Debug.LogWarning("Missing buttonPrefab or Area_CAPTCHA reference!");
            return;
        }

        foreach (float y in yPositions)
        {
            GameObject newButton = Instantiate(buttonPrefab, Area_CAPTCHA);
            buttons.Add(newButton);
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
                buttonsPressed++;
                if (buttonsPressed >= 3) CAPTCHA_Success();
            });
        }
    }

    void Spawn_HardCaptcha()
    {
        if (buttonPrefab == null || Area_CAPTCHA == null)
        {
            Debug.LogWarning("Missing buttonPrefab or Area_CAPTCHA reference!");
            return;
        }

        for (int i = 0; i < yPositions.Length; i++)
        {
            // Spawn a copy of the prefab as a child of the parent
            GameObject newButton = Instantiate(buttonPrefab, Area_CAPTCHA);
            buttons.Add(newButton);
            RectTransform rect = newButton.GetComponent<RectTransform>();


            // Random start position within panel
            float halfWidth = Area_CAPTCHA.rect.width / 2f;
            float halfHeight = Area_CAPTCHA.rect.height / 2f;
            Vector2 startPos = new Vector2(
                Random.Range(-halfWidth + 50f, halfWidth - 50f),
                Random.Range(-halfHeight + 50f, halfHeight - 50f)
            );

            rect.anchoredPosition = startPos;
            rect.localScale = Vector3.one;

            // Add bouncing mover
            ButtonBouncer mover = newButton.AddComponent<ButtonBouncer>();
            mover.Init(Area_CAPTCHA, speed);

            // Disable on click
            Button btn = newButton.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                btn.interactable = false;
                mover.enabled = false;
                buttonsPressed++;
                if (buttonsPressed >= 3) CAPTCHA_Success();
            });
        }
    }

    void CAPTCHA_Success()
    {
        events.New_Notification(0, "App_Name", "CAPTCHA confirmed. Logging in...");

        CAPTCHA_Leave();
    }

    public void CAPTCHA_Leave()
    {
        Page_CAPTCHA.transform.DOMove(closedPos.position, transitionTime).SetEase(Ease.OutCubic);

        buttonsPressed = 0;

        for (int i = 0; i < buttons.Count; i++)
        {
            Destroy(buttons[i].gameObject);
        }
    }

    public void CAPTCHA_Start()
    {
        Page_CAPTCHA.transform.DOMove(openedPos.position, transitionTime).SetEase(Ease.OutCubic);

        if (Easy) Spawn_EasyCaptcha();
        else if (Medium) Spawn_MediumCaptcha();
        else if (Hard) Spawn_HardCaptcha();
        else this.gameObject.SetActive(false);
    }
}