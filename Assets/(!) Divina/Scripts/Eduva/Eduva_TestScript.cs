using UnityEngine;
using UnityEngine.UI;

public class Eduva_TestScript : MonoBehaviour
{
    [System.Serializable]
    public struct AnswerGroup
    {
        public Toggle Answer1;
        public Toggle Answer2;
        public Toggle Answer3;
        public Toggle Answer4;
    }

    [SerializeField] public GameObject Test;
    [SerializeField] private Toggle testDone;
    [SerializeField] private Button submitButton;
    [SerializeField] private AnswerGroup[] answers; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (submitButton != null)
            submitButton.interactable = false;

        if (testDone != null)
            testDone.isOn = false;

        if (answers != null)
        {
            for (int i = 0; i < answers.Length; i++)
            {
                var group = answers[i];
                var toggles = GetGroupToggles(group);

                for (int j = 0; j < toggles.Length; j++)
                {
                    var t = toggles[j];
                    int gi = i;
                    int ti = j;

                    // ensure toggles start off
                    if (t != null)
                        t.isOn = false;

                    UnityEngine.Events.UnityAction<bool> action = (isOn) =>
                    {
                        // if this toggle was turned on, enforce mutual exclusion in the group
                        if (isOn)
                            OnGroupToggleSelected(gi, ti);

                        // always update submit button state after any change
                        UpdateSubmitInteractable();
                    };

                    if (t != null)
                        t.onValueChanged.AddListener(action);
                }

                // listeners added to toggles directly; not stored here
            }
        }

        // ensure submit button state reflects initial toggle values (in case toggles are preset in inspector)
        UpdateSubmitInteractable();
    }

    private void UpdateSubmitInteractable()
    {
        if (submitButton == null)
            return;

        if (answers == null || answers.Length == 0)
        {
            submitButton.interactable = false;
            return;
        }

        for (int i = 0; i < answers.Length; i++)
        {
            var toggles = GetGroupToggles(answers[i]);
            bool anyOn = false;
            for (int j = 0; j < toggles.Length; j++)
            {
                var t = toggles[j];
                if (t != null && t.isOn)
                {
                    anyOn = true;
                    break;
                }
            }

            if (!anyOn)
            {
                submitButton.interactable = false;
                return;
            }
        }

        submitButton.interactable = true;
    }

    private Toggle[] GetGroupToggles(AnswerGroup g)
    {
        return new Toggle[] { g.Answer1, g.Answer2, g.Answer3, g.Answer4 };
    }

    private void OnGroupToggleSelected(int groupIndex, int selectedIndex)
    {
        if (answers == null || groupIndex < 0 || groupIndex >= answers.Length)
            return;

        var toggles = GetGroupToggles(answers[groupIndex]);

        for (int k = 0; k < toggles.Length; k++)
        {
            if (k == selectedIndex)
                continue;

            var t = toggles[k];
            if (t != null && t.isOn)
            {
                // turning this off will trigger its onValueChanged, but handler ignores when isOn==false
                t.isOn = false;
            }
        }
    }

    // Note: listeners are not removed individually here. If you need to preserve
    // other listeners on the toggles, consider storing delegates or using
    // ToggleGroup components instead.

    // Update is called once per frame
    void Update()
    {
        
    }
}
