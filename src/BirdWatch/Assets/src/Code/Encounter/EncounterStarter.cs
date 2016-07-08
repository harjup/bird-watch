using UnityEngine;

public class EncounterStarter : Singleton<EncounterStarter>
{
    public string Bird = "None";
    
    public void Init(string bird)
    {
        Bird = bird;
        LevelLoader.Instance.LoadLevel(Level.Encounter);
    }
}
