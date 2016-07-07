using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Bootstrapper : MonoBehaviour
{
    private static bool _initialized;

    private GameObject ServiceRoot;

    private GameObject CutsceneActions;

    public void Awake()
    {
        if (_initialized == false)
        {
            ServiceRoot = new GameObject("ServiceRoot");
            DontDestroyOnLoad(ServiceRoot);

            GeneratePrefabs();

            GenerateServices();
            _initialized = true;
        }
    }

    private void GeneratePrefabs()
    {
        SpawnPersistentPrefab("Prefabs/Canvas");
    }

    // Make sure these are all monobehaviors or bad things will happen
    // These are all services that need to exist somewhere and require no configuration
    private List<Type> SimpleServices = new List<Type>
    {
        typeof(LevelLoader),
        typeof(EncounterStarter)
    };

    private void GenerateServices()
    {
        foreach (var expectedService in SimpleServices)
        {
            ServiceRoot.AddComponent(expectedService);
        }
        
        var singlePrefabs = Resources.LoadAll<GameObject>("Prefabs/Services");
        
        foreach (var singlePrefab in singlePrefabs)
        {
            var result = Instantiate(singlePrefab);
            result.transform.parent = ServiceRoot.transform;
            DontDestroyOnLoad(result);
        }
    }

    private GameObject SpawnPersistentPrefab(string path)
    {

        var prefab = Resources.Load<GameObject>(path);
        var newInstance = Instantiate(prefab);
        newInstance.name = prefab.name;
        DontDestroyOnLoad(newInstance);
        return newInstance;
    }
}
