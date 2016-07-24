using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TextDisplayGui : MonoBehaviour
{

    private GameObject _dialogueWindow;
    private TextCrawler _textCrawler;
    private DialogueWindowText _dialogueWindowText;
    private Text _dialogueWindowName;
    private GameObject _dialogChoicePrefab;


    // Gonna be lazy about initialization with lazy initialization. Can't trust start to run first anyways.
    private GameObject DialogueWindow 
    { 
        get
        {
            if (_dialogueWindow == null)
            {
                return _dialogueWindow = transform.FindChild("DialogWindow").gameObject;
            } 
            return _dialogueWindow;
        } 
    }

    private TextCrawler TextCrawler
    {
        get
        {
            if (_textCrawler == null)
            {
                return _textCrawler = gameObject.GetComponent<TextCrawler>();
            } 
            return _textCrawler;
        }
    }

    private DialogueWindowText DialogueWindowText
    {
        get
        {
            if (_dialogueWindowText == null)
            {
                return _dialogueWindowText = gameObject.GetComponentInChildren<DialogueWindowText>();
            }

            return _dialogueWindowText;
        }
    }

    private Text DialogueWindowName
    {
        get
        {
            if (_dialogueWindowName == null)
            {
                return _dialogueWindowName = GameObject.Find("DialogueWindowName").GetComponent<Text>();
            }

            return _dialogueWindowName;
        }
    }

    private GameObject DialogChoicePrefab
    {
        get
        {
            if (_dialogChoicePrefab == null)
            {
                return _dialogChoicePrefab = Resources.Load<GameObject>("Prefabs/DialogChoiceButton");
            }

            return _dialogChoicePrefab;
        }
    }

    public void Awake()
    {
        // Hide self by default
        DialogueWindowText.SetText("");
        DialogueWindow.SetActive(false);
    }

    // This is an IEnumerator so we can wait for animations
    public IEnumerator ShowDialogueWindow()
    {
        // Do a tween animation and wait for it to finish
        
        // For now just make it draw
        DialogueWindow.SetActive(true);

        yield return null;
    }


    public void SetName(string name)
    {
        DialogueWindowName.text = name;
    }

    public IEnumerator CrawlText(string text, Action callback)
    {        
        yield return StartCoroutine(TextCrawler.TextCrawl(text, DialogueWindowText.SetText));

        callback();
    }

    public void SkipTextCrawl()
    {
        if (TextCrawler._inProcess)
        {
            TextCrawler.SkipToEnd();
        }
    }

    public IEnumerator HideDialogWindow()
    {
        _dialogueWindowText.SetText("");
        TextCrawler.Clear();

        DialogueWindow.SetActive(false);

        yield return null;
    }


    public void ShowChoices(List<string> choices, Action<int> onChoice)
    {
        //TODO: Make this safer~
        var initialPosition = GameObject.Find("ChoiceInitialPosition").transform.position;

        // TODO: Refactor b/c this is copy pasted
        var buttons = new List<GameObject>();
        for (int i = 0; i < choices.Count; i++)
        {
            var choice = choices[i];
            var button = Instantiate(DialogChoicePrefab);
            button.transform.parent = transform.parent.Find("Buttons");
            button.transform.localScale = Vector3.one; // The scale is getting messed up for some reason??

            button.transform.position = new Vector2(initialPosition.x, initialPosition.y - ((Screen.height / 6f) * i));
            button.GetComponentInChildren<Text>().text = choice;

            var choiceIndex = i; // Curse you C# mutability!

            buttons.Add(button);

            button.GetComponent<Button>()
                .onClick
                .AddListener(() =>
                {
                    CleanUpButtons(buttons); // This is gross, but should be called when all the buttons are in the list.
                    onChoice(choiceIndex);
                });
        }
    }

    public IEnumerator ShowChoicesAndWait(List<string> choices, Action<int> onChoice)
    {
        var waitingForChoice = true;
        ShowChoices(choices, i =>
        {
            onChoice(i);
            waitingForChoice = false;
        });

        while (waitingForChoice)
        {
            yield return null;
        }
    }



    private void CleanUpButtons(List<GameObject> buttons)
    {
        foreach (var button in buttons)
        {
            Destroy(button);
        }
    }

}
