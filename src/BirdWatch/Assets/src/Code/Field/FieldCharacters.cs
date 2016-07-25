using UnityEngine;
using System.Collections;
using System.Linq;

public class FieldCharacters : MonoBehaviour
{
    private Animator _juddAnimator;
    private Animator _gregAnimator;

    private Animator _juddEffectAnimator;
    private Animator _gregEffectAnimator;

    private bool _initRun = false;

    private void Init()
    {
        if (_initRun)
        {
            return;
        }

        var animators = GetComponentsInChildren<Animator>();

        _juddAnimator = animators.First(a => a.name == "Judd");
        _gregAnimator = animators.First(a => a.name == "Greg");

        _juddEffectAnimator = animators.First(a => a.name == "Judd-Effect");
        _gregEffectAnimator = animators.First(a => a.name == "Greg-Effect");

        _initRun = true;
    }

    private string currentCharacter;
    public void Talk(string character)
    {
        if (currentCharacter == character)
        {
            return;
        }

        currentCharacter = character;

        ClearTalk();

        if (character.ToLower() == "judd")
        {
            _juddEffectAnimator.CrossFade("Talk", 0);
        }

        if (character.ToLower() == "greg")
        {
            _gregEffectAnimator.CrossFade("Talk", 0);
        }
    }

    public void ClearTalk()
    {
        _juddEffectAnimator.CrossFade("Hide", 0);
        _gregEffectAnimator.CrossFade("Hide", 0);
    }



    public void Walk()
    {
        Init();

        _juddAnimator.CrossFade("Walk", 0);
        _gregAnimator.CrossFade("Walk", 0);
    }

    public void Idle()
    {
        Init();

        _juddAnimator.CrossFade("Idle", 0);
        _gregAnimator.CrossFade("Idle", 0);
    }
}
