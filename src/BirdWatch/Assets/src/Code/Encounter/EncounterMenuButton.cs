using System;
using UnityEngine;
using System.Collections;

public class EncounterMenuButton : MonoBehaviour
{
    public enum Type
    {
        Unknown,
        Breath,
        Camera,
        Book,
        Exit
    }

    public Type ButtonType;

    private SpriteRenderer _render;
    private Color initialColor;

    // Use this for initialization
    void Start()
    {
        _render = GetComponent<SpriteRenderer>();
        initialColor = _render.color;
    }

    // Mouse is depressed
    public void OnMouseDown()
    {
        _render.color = new Color(.7f, .7f, .7f, 1f);
    }

    // Successful mouseclick
    public void OnMouseUpAsButton()
    {
        _render.color = initialColor;

        var actionSelect = GetComponentInParent<ActionSelect>();
        if (actionSelect.Disabled)
        {
            return;
        }


        switch (ButtonType)
        {
            case Type.Breath:
                StartCoroutine(FindObjectOfType<EncounterRunner>().RunBreathingMinigame());
                break;
            case Type.Camera:
                StartCoroutine(FindObjectOfType<EncounterRunner>().RunCameraMinigame());
                break;
            case Type.Book:
                StartCoroutine(FindObjectOfType<EncounterRunner>().RunBookMinigame());
                break;
            case Type.Exit:
                StartCoroutine(FindObjectOfType<EncounterRunner>().RunExitChoice());
                break;
        }

        
    }

    // Mouse over
    public void OnMouseEnter()
    {
        _render.color = Color.grey;
    }

    // Mouse off
    public void OnMouseExit()
    {
        _render.color = initialColor;
    }
}
