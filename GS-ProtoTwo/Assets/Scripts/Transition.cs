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
    [SerializeField] private string nextSceneToLoad;
    public string transitionMessage = "FIGHT!";
    [SerializeField] private TextMeshProUGUI logoText;

    private void Awake()
    {
        logoText.text = transitionMessage;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator StartTransition()
    {
        animator.SetTrigger(PlayTransition);

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(nextSceneToLoad);
    }
}
