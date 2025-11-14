using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Eduva_DisableButton : MonoBehaviour
{
    [Header("UI to change")]
    [SerializeField] private TextMeshProUGUI quizStatus;
    [SerializeField] private Toggle quizToggle;

    public void disableButton(Button OpenQuiz)
    {
        OpenQuiz.interactable = false;
        quizStatus.text = "Quiz Status: Finished";
    }
}
