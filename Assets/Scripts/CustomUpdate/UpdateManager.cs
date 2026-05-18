using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    private static UpdateManager instance;
    public static UpdateManager Instance => instance;

    private readonly List<IUpdatable> updatablesList = new List<IUpdatable>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Register(IUpdatable updatable)
    {
        if (!updatablesList.Contains(updatable))
        { 
            updatablesList.Add(updatable); 
        }
    }

    public void Unregister(IUpdatable updatable)
    {
        updatablesList.Remove(updatable);
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        foreach(var updatable in updatablesList)
        {
            updatable.CustomUpdate(deltaTime);
        }
    }
}
