using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Transition : MonoBehaviour
{
    public Animator animator;

    private static readonly int PlayTransition = Animator.StringToHash("PlayTransition");
    public string nextSceneToLoad;
    public string combatMessage = "FIGHT!";
    public string worldMessage = "";
    private GameObject logo;
    private TextMeshProUGUI logoText;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        logo = transform.GetChild(2).gameObject;
        logoText = logo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        UpdateText(worldMessage);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator TransitionToCombat()
    {
        UpdateText(combatMessage);
        animator.SetTrigger(PlayTransition);

        yield return new WaitForSeconds(1.5f);

        SceneTransition.instance.MoveToCombat();
    }
    
    public IEnumerator TransitionToWorld()
    {
        UpdateText(worldMessage);
        animator.SetTrigger(PlayTransition);

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("JacksWorld");
    }

    public void UpdateText(string message)
    {
        logoText.text = message;
        logo.SetActive(message != "");
    }
}
