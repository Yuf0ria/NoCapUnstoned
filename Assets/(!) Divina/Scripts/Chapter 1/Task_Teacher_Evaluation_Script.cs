using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Task_Teacher_Evaluation_Script : MonoBehaviour
{
    [Header("Task")]
    [SerializeField] private bool isDone;
    [SerializeField] private int profEvaluated;

    [Header("Professors")]
    [SerializeField] private TextMeshProUGUI profMargot;
    [SerializeField] private Button profMargotButton;

    [SerializeField] private TextMeshProUGUI profJacob;
    [SerializeField] private Button profJacobButton;

    [SerializeField] private TextMeshProUGUI profAngel;
    [SerializeField] private Button profAngelButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isDone = false;
        profEvaluated = 0;

        profMargotButton.onClick.AddListener(() =>
        {
            profIsEvaluated(profMargot);
        });
        profJacobButton.onClick.AddListener(() =>
        {
            profIsEvaluated(profJacob);
        });
        profAngelButton.onClick.AddListener(() =>
        {
            profIsEvaluated(profAngel);
        });
    }

    void profIsEvaluated(TextMeshProUGUI prof)
    {
        profEvaluated++;
        prof.fontStyle = FontStyles.Strikethrough;

        if (profEvaluated == 3)
        {
            allProfEvaluated();
        }
    }

    void allProfEvaluated()
    {
        isDone = true;

        //Changes the font style of the Notes preview to strikethrough to show that it's done
        TextMeshProUGUI noteTitle = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI noteDesc = transform.Find("Description").GetComponent<TextMeshProUGUI>();

        noteTitle.fontStyle = FontStyles.Strikethrough;
        noteDesc.fontStyle = FontStyles.Strikethrough;
    }
}
