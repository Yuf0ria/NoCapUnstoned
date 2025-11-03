using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UNFINISHED AS FRICK!
/// </summary>

public class AttacksScript : MonoBehaviour
{
    [SerializeField] private GameObject[] attack; //The malware is activated based on whether or not the gameobject is active through OnEnable

    private Button hyperlink;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hyperlink = GetComponent<Button>(); //gets hyperlink button (the script is inside the gameobject)

        int attackType;
        attackType = Random.Range(0, attack.Length);
        hyperlink.onClick.AddListener(delegate { randomizeAttack(attackType); }); //
    }

    public void randomizeAttack(int attackType) //Randomizes attack based on what number is generated when clicking the hyperlink.
    {
        Debug.Log("WUH OH NOT SAFE!");

        if (attack != null)
        {
            attack[attackType].SetActive(true);
            Debug.Log("Attack no. :" + attackType + " || " + attack[attackType].name + " is active");
        }
    }
}
