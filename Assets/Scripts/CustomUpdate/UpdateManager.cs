using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    private static UpdateManager instance;
    public static UpdateManager Instance => instance;

    private readonly List<IStarteable> starteablesList = new List<IStarteable>();
    private readonly List<IUpdateable> updateablesList = new List<IUpdateable>();
    private readonly List<IFixedUpdateables> fixedUpdateablesList = new List<IFixedUpdateables>();


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

    #region Registros

    public void Register(IStarteable starteable)
    {
        if (!starteablesList.Contains(starteable))
        {
            starteablesList.Add(starteable);
        }
    }

    public void Unregister(IStarteable starteable)
    {
        starteablesList.Remove(starteable);
    }

    public void Register(IUpdateable updatable)
    {
        if (!updateablesList.Contains(updatable))
        {
            updateablesList.Add(updatable);
        }
    }

    public void Unregister(IUpdateable updatable)
    {
        updateablesList.Remove(updatable);
    }

    public void Register(IFixedUpdateables fUpdateable)
    {
        if (!fixedUpdateablesList.Contains(fUpdateable))
        {
            fixedUpdateablesList.Add(fUpdateable);
        }
    }

    public void Unregister(IFixedUpdateables fUpdatable)
    {
        fixedUpdateablesList.Remove(fUpdatable);
    }
    #endregion
    
    private void Start()
    {
        foreach (var starteable in starteablesList)
        {
            starteable.CustomStart();
            
        }
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        foreach (var updatable in updateablesList)
        {
            updatable.CustomUpdate(deltaTime);
        }
    }

    private void FixedUpdate()
    {
        foreach (var updatable in fixedUpdateablesList)
        {
            updatable.CustomFixedUpdate();
        }
    }
}
