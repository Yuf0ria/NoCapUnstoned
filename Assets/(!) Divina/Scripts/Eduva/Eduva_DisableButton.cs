using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Eduva_DisableButton : MonoBehaviour
{
    [Header("UI to change")]
    [SerializeField] private TextMeshProUGUI quizStatus;
    [SerializeField] private Toggle quizToggle;

    private void Start()
    {
        quizToggle.isOn = false;
    }

    public void disableButton(Button OpenQuiz)
    {
        OpenQuiz.interactable = false;
        quizToggle.isOn = true;
        quizStatus.text = "Quiz Status: Finished";
    }
}
