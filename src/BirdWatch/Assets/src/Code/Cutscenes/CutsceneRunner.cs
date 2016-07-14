using UnityEngine;
using System.Collections;
using Yarn.Unity;

public class CutsceneRunner : MonoBehaviour
{
    void Start()
    {
        var dialogRoot = "Intermission_";
        var dayIndex = GameProgress.Instance.CurrentDay.ToString("00");
        var currentNode = dialogRoot + dayIndex;

        FindObjectOfType<DialogueRunner>().StartDialogue(currentNode);
    }
}
