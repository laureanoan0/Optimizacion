using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    private static UpdateManager instance;
    public static UpdateManager Instance => instance;

    private readonly List<IUpdateable> updatablesList = new List<IUpdateable>();

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    public void Register(IUpdateable updatable)
    {
        if (!updatablesList.Contains(updatable))
        { 
            updatablesList.Add(updatable); 
        }
    }

    public void Unregister(IUpdateable updatable)
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
