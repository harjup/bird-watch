using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text.RegularExpressions;
using Assets.src.Code;
using UnityEngine.EventSystems;
using Yarn;
using Yarn.Unity;

public class MyYarnTheme : Yarn.Unity.DialogueUIBehaviour
{
    private readonly Regex _stringInterpolationRegex = new Regex(@"(\$[A-Za-z_\.]+)");

    
    private TextDisplayGui _textDisplayGui;

    private TextDisplayGui TextDisplayGui
    {
        get
        {
            if (_textDisplayGui == null)
            {
                return _textDisplayGui = FindObjectOfType<TextDisplayGui>();
            }

            return _textDisplayGui;
        }
    }

    private Yarn.OptionChooser SetSelectedOption;
		
    public override IEnumerator RunLine(Line line)
    {
        yield return StartCoroutine(TextDisplayGui.ShowDialogueWindow());
        
        var text = line.text;

        #region DisabledVaribleInterpolationIgnoreMePls
        // TODO: This is currently a hack that gets variable interpolation working
        // Hopefully this will be officially supported soon!!!
        // ...or I can eventually do it.
        // TODO: Only enable if I actually need this for some reason
        //        var variableStorage = FindObjectOfType<ExampleVariableStorage>();
        //        MatchCollection matches = _stringInterpolationRegex.Matches(text);
        //        foreach (Match match in matches)
        //        {
        //            Group group = match.Groups[0];
        //            var key = group.Value;
        //            Debug.Log(key);
        //            
        //            var value = variableStorage.GetValue(key);
        //            Debug.Log(value);
        //
        //            text = text.Replace(key, value.AsString);
        //        }
        #endregion

        StartCoroutine(InputWait.WaitForInputAxis("Fire1", TextDisplayGui.SkipTextCrawl));

        // If we've got a name to use, then set it here.
        var splitOnName = text.Split(new char[] {':'}, StringSplitOptions.RemoveEmptyEntries);
        if (splitOnName.Length > 1)
        {
            TextDisplayGui.SetName(splitOnName[0]);

            var remainder = string.Join(":", splitOnName.Skip(1).ToArray()).Trim(); 

            yield return StartCoroutine(TextDisplayGui.CrawlText(remainder, () => { }));
        }
        else
        {
            TextDisplayGui.SetName("");
            yield return StartCoroutine(TextDisplayGui.CrawlText(text, () => { }));
        }
        

        // Wait for input before advancing
        yield return StartCoroutine(InputWait.WaitForInputAxis("Fire1", () => {}));

        //yield return StartCoroutine(TextDisplayGui.HideDialogWindow());
    }

    public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
    {
        yield return StartCoroutine(TextDisplayGui.HideDialogWindow());

        // Record that we're using it
        SetSelectedOption = optionChooser;

        TextDisplayGui.ShowChoices(optionsCollection.options.ToList(), SetOption);

        // Wait until the chooser has been used and then removed (see SetOption below)
        while (SetSelectedOption != null)
        {
            yield return null;
        }
    }

    public void SetOption(int selectedOption)
    {
        // Call the delegate to tell the dialogue system that we've
        // selected an option.
        SetSelectedOption(selectedOption);

        // Now remove the delegate so that the loop in RunOptions will exit
        SetSelectedOption = null;
    }

    public override IEnumerator RunCommand(Command command)
    {
        // Commands that follow [verb] [gameobject] [args] will be automagically run by yarnSpinner.
        // This override is for commands that don't match up with anything!
        // Most of the content here is for finding commands that follow my ~special alternate syntax~: await [verb] [gameobject] [args]

        Debug.Log("Running in MyYarnTheme Command: " + command.text);

        var parts = command.text.Split(' ');

        var commandType = parts[0];
        if (commandType == "await")
        {
            yield return StartCoroutine(TextDisplayGui.HideDialogWindow());

            var method = parts[1];
            var target = parts[2];
            var args = parts.Skip(3).ToArray();

            var components = AwaitCommandFinder.FindAwaitableCommands(method, target, args);

            Debug.Log("Found " + components.Length + " instances of AwaitableYarnCommand " + command.text);

            foreach (var methodInfo in components)
            {

                //var targetGo = GameObject.Find(target);
                var targetType = methodInfo.Type;
                var info = methodInfo.MethodInfo;

                var targGo = GameObject
                    .Find(target)
                    .GetComponent(targetType);

                var result = info.Invoke(targGo, args) as IEnumerator;

                yield return StartCoroutine(result);
            }

            yield break;
        }

        Debug.LogError("Command: " + command.text + " did not match up with anything!!!");

        yield return null;
    }

    // Callback for yarn starting a dialogue
    public override IEnumerator DialogueStarted()
    {
        yield return null;
    }

    // Yay we're done. Called when the dialogue system has finished running.
    public override IEnumerator DialogueComplete()
    {
        yield return StartCoroutine(TextDisplayGui.HideDialogWindow());

        yield return null;
    }
}
