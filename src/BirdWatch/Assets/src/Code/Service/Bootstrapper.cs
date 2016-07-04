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

            CutsceneActions = new GameObject("CutsceneActions");
            DontDestroyOnLoad(CutsceneActions);

            SpawnSpecialPrefabs();
            SpawnCutsceneActions();
            GenerateServices();
            _initialized = true;
        }
    }

    // Make sure these are all monobehaviors or bad things will happen
    // These are all services that need to exist somewhere and require no configuration
    private List<Type> SimpleServices = new List<Type>
    {
        typeof(LevelLoader)
    };

    private void SpawnSpecialPrefabs()
    {
        var player           = Resources.Load<GameObject>("Prefabs/Player");
        var theCamera = Resources.Load<GameObject>("Prefabs/Camera");
        var canvas           = Resources.Load<GameObject>("Prefabs/Canvas");
        
        var playerObject = Instantiate(player);
        var cameraObj = Instantiate(theCamera);
        var canvasObj =  Instantiate(canvas);

        // Unity does this really annoything thing where it appends "clone" to the end of instantiated prefabs.
        playerObject.name = player.name;
        cameraObj.name = theCamera.name;
        canvasObj.name = canvas.name;
        
        DontDestroyOnLoad(playerObject);
        DontDestroyOnLoad(cameraObj);
        DontDestroyOnLoad(canvasObj);
    }

    private void SpawnCutsceneActions()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Level");
        var obj = Instantiate(prefab);
        obj.name = prefab.name;
        DontDestroyOnLoad(obj);

        obj.transform.parent = CutsceneActions.transform;

        prefab = Resources.Load<GameObject>("Prefabs/Timer");
        obj = Instantiate(prefab);
        obj.name = prefab.name;
        DontDestroyOnLoad(obj);

        obj.transform.parent = CutsceneActions.transform;

        prefab = Resources.Load<GameObject>("Prefabs/Music");
        obj = Instantiate(prefab);
        obj.name = prefab.name;
        DontDestroyOnLoad(obj);
        
        obj.transform.parent = CutsceneActions.transform;
    }

    private void GenerateServices()
    {
        foreach (var expectedService in SimpleServices)
        {
            ServiceRoot.AddComponent(expectedService);
        }
        
        var singlePrefabs = Resources.LoadAll<GameObject>("ServicePrefabs");
        
        foreach (var singlePrefab in singlePrefabs)
        {
            var result = Instantiate(singlePrefab);
            result.transform.parent = ServiceRoot.transform;
            DontDestroyOnLoad(result);
        }
    }
}
