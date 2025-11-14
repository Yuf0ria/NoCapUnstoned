using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Settings_UI : MonoBehaviour
{
    [SerializeField] private Button Toggle;
    [SerializeField] private float duration = 0.3f;

    [Header("Color Toggle")]
    [SerializeField] private Color[] toggleColor;
    [SerializeField] private Graphic targetGraphic;

    private int colorIndex = 0;
    private bool listenerAdded = false;

    private void Start()
    {
        if (targetGraphic == null)
            targetGraphic = GetComponent<Graphic>();

        if (toggleColor != null && toggleColor.Length > 0 && targetGraphic != null)
        {
            colorIndex = 0; 
            for (int i = 0; i < toggleColor.Length && i < 2; i++)
            {
                if (toggleColor[i].a <= 0.01f)
                {
                    toggleColor[i].a = 1f;
                }
            }
            targetGraphic.color = toggleColor[0];
        }

        if (Toggle != null)
        {
            bool hasPersistent = false;
            int persistentCount = Toggle.onClick.GetPersistentEventCount();
            for (int i = 0; i < persistentCount; i++)
            {
                if (Toggle.onClick.GetPersistentTarget(i) == this && Toggle.onClick.GetPersistentMethodName(i) == "ToggleButton")
                {
                    hasPersistent = true;
                    break;
                }
            }

            if (!hasPersistent)
            {
                Toggle.onClick.AddListener(ToggleButton);
                listenerAdded = true;
            }
        }
    }

    private void OnDestroy()
    {
        if (Toggle != null && listenerAdded)
            Toggle.onClick.RemoveListener(ToggleButton);
    }

    // Provide access to the Button so other scripts can subscribe without requiring direct field access
    public Button ToggleButtonRef => Toggle;

    public void ToggleButton()
    { 
        // Move the UI RectTransform to the mirrored X (left/right toggle)
        var rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            float currentX = rt.anchoredPosition.x;
            float targetX = -currentX;
            rt.DOAnchorPosX(targetX, duration).SetEase(Ease.OutQuad);
        }

        // Color toggle
        if (toggleColor != null && toggleColor.Length >= 2 && targetGraphic != null)
        {
            int nextIndex = (colorIndex == 0) ? 1 : 0;
            Color targ = toggleColor[nextIndex];
            // THEY HAVE DOCOLOR?!
            targetGraphic.DOColor(targ, duration).SetEase(Ease.OutQuad);
            colorIndex = nextIndex;
        } 
    }
    
}
