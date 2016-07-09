using UnityEngine;
using System.Collections;

public class EncounterMenuGui : Singleton<EncounterMenuGui>
{

    private GameObject MenuContainer;

    public void Start()
    {
        MenuContainer = transform.Find("MenuContainer").gameObject;
        MenuContainer.SetActive(false);
    }

    public void Enable()
    {
        MenuContainer.SetActive(true);
    }



}
