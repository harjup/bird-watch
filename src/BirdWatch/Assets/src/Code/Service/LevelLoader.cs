using UnityEngine;
using System.Collections;

// ReSharper disable InconsistentNaming
public enum Level
{
    Field,
    Encounter,
    Cutscene
}
// ReSharper enable InconsistentNaming

public class LevelLoader : Singleton<LevelLoader>
{
    public void LoadLevelAsString(string levelName)
    {
        Application.LoadLevel(levelName);
    }

    public void LoadLevel(Level levelEntrance)
    {
        switch (levelEntrance)
        {
            case Level.Field:
                Application.LoadLevel("Field");
                break;
            case Level.Encounter:
                Application.LoadLevel("Encounter");
                break;
            case Level.Cutscene:
                Application.LoadLevel("Cutscene");
                break;
            default:
                Debug.LogError("Level entrance type " + levelEntrance + " not supported");
                break;
        }
    }

    public void LoadDance()
    {
        Application.LoadLevel("Dance");
    }
}
