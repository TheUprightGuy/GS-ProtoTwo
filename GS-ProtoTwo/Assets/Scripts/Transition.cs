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
    public string transitionMessage = "FIGHT!";
    private GameObject logo;
    private TextMeshProUGUI logoText;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        logo = transform.GetChild(2).gameObject;
        logoText = logo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        logoText.text = transitionMessage;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator TransitionToCombat()
    {
        logo.SetActive(true);
        animator.SetTrigger(PlayTransition);

        yield return new WaitForSeconds(1.5f);

        SceneTransition.instance.MoveToCombat();
    }
    
    public IEnumerator TransitionToWorld()
    {
        logo.SetActive(false);
        animator.SetTrigger(PlayTransition);

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("JacksWorld");
    }
}
