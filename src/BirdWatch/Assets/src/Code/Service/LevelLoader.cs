using UnityEngine;
using System.Collections;

// ReSharper disable InconsistentNaming
public enum LevelEntrance
{

}
// ReSharper enable InconsistentNaming

public class LevelLoader : Singleton<LevelLoader>
{
    public void LoadLevelAsString(string levelName)
    {
        Application.LoadLevel(levelName);
    }

    public void LoadLevel(LevelEntrance levelEntrance)
    {
        switch (levelEntrance)
        {
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
