using System;
using UnityEngine;
using System.Collections;

public class EncounterMenuButton : MonoBehaviour
{
    public enum Type
    {
        Unknown,
        Breath,
        Camera
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
        _render.color = Color.black;
    }

    // Successful mouseclick
    public void OnMouseUpAsButton()
    {
        _render.color = initialColor;


        switch (ButtonType)
        {
            case Type.Breath:
                StartCoroutine(FindObjectOfType<EncounterRunner>().RunMinigame<BreathingMinigame>());
                break;
            case Type.Camera:
                //StartCoroutine(FindObjectOfType<EncounterRunner>().RunMinigame(ButtonType));
                break;
        }

        
    }

    // Mouse over
    public void OnMouseEnter()
    {
        _render.color = Color.blue;
    }

    // Mouse off
    public void OnMouseExit()
    {
        _render.color = initialColor;
    }
}
