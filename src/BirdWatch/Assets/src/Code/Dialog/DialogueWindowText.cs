using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueWindowText : MonoBehaviour
{
    private Text _text;

    public Text Text
    {
        get
        {
            if (_text == null)
            {
                return _text = GetComponent<Text>(); 
            }

            return _text;
        }
    }

    public void SetText(string value)
    {
        Text.text = value;
    }
}
