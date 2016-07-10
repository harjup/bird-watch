using UnityEngine;

public class EncounterStarter : Singleton<EncounterStarter>
{
    public Bird Bird { get; private set; }
    
    public void Init(Bird bird)
    {
        Bird = bird;
        LevelLoader.Instance.LoadLevel(Level.Encounter);
    }
}
