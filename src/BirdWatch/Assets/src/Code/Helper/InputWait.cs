using System;
using UnityEngine;
using System.Collections;

public static class InputWait
{
    // Just gonna dump these here for now.
    // Not a fan of Input.GetKey being a static method I can call from wherever I want :\

    public static IEnumerator WaitForInputDown(KeyCode keycode, Action callback)
    {
        // If the player was holding the button
        var buttonPressedOnEnter = Input.GetKey(keycode);
        while (buttonPressedOnEnter)
        {
            buttonPressedOnEnter = Input.GetKey(keycode);

            yield return null;
        }

        while (true)
        {
            if (Input.GetKeyDown(keycode))
            {
                callback();
                break;
            }

            yield return null;
        }
    }

    public static IEnumerator WaitForInputAxis(string axis, Action callback)
    {
        var tolerance = .01f;
        Func<float, bool> aboveTolerance = (f) => f > tolerance;

        // If the player was holding the button
        while (aboveTolerance(Input.GetAxis(axis)))
        {
            yield return null;
        }

        while (true)
        {
            if (aboveTolerance(Input.GetAxis(axis)))
            {
                callback();
                break;
            }

            yield return null;
        }
    }
}
