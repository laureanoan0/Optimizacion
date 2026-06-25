using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-100)]
public class UpdateManager : MonoBehaviour
{
    private static UpdateManager instance;
    public static UpdateManager Instance => instance;

    private readonly List<IStarteable> starteablesList = new List<IStarteable>();
    private readonly List<IUpdateable> updateablesList = new List<IUpdateable>();
    private readonly List<IFixedUpdateables> fixedUpdateablesList = new List<IFixedUpdateables>();

 
    private readonly List<IStarteable> starteablesToAdd = new List<IStarteable>();
    private readonly List<IStarteable> starteablesToRemove = new List<IStarteable>();
    private readonly List<IUpdateable> updateablesToAdd = new List<IUpdateable>();
    private readonly List<IUpdateable> updateablesToRemove = new List<IUpdateable>();
    private readonly List<IFixedUpdateables> fixedUpdateablesToAdd = new List<IFixedUpdateables>();
    private readonly List<IFixedUpdateables> fixedUpdateablesToRemove = new List<IFixedUpdateables>();

    private bool isStarting;
    private bool isUpdating;
    private bool isFixedUpdating;

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
        if (isStarting)
        {
            if (!starteablesToAdd.Contains(starteable))
            {
                starteablesToAdd.Add(starteable);
            }
            return;
        }

        if (!starteablesList.Contains(starteable))
        {
            starteablesList.Add(starteable);
        }
    }
    public void Unregister(IStarteable starteable)
    {
        if (isStarting)
        {
            starteablesToRemove.Add(starteable);
            return;
        }

        starteablesList.Remove(starteable);
    }

    public void Register(IUpdateable updatable)
    {
        if (isUpdating)
        {
            if (!updateablesToAdd.Contains(updatable))
            {
                updateablesToAdd.Add(updatable);
            }
            return;
        }

        if (!updateablesList.Contains(updatable))
        {
            updateablesList.Add(updatable);
        }
    }
    public void Unregister(IUpdateable updatable)
    {
        if (isUpdating)
        {
            updateablesToRemove.Add(updatable);
            return;
        }

        updateablesList.Remove(updatable);
    }

    public void Register(IFixedUpdateables fUpdateable)
    {
        if (isFixedUpdating)
        {
            if (!fixedUpdateablesToAdd.Contains(fUpdateable))
            {
                fixedUpdateablesToAdd.Add(fUpdateable);
            }
            return;
        }

        if (!fixedUpdateablesList.Contains(fUpdateable))
        {
            fixedUpdateablesList.Add(fUpdateable);
        }
    }
    public void Unregister(IFixedUpdateables fUpdatable)
    {
        if (isFixedUpdating)
        {
            fixedUpdateablesToRemove.Add(fUpdatable);
            return;
        }

        fixedUpdateablesList.Remove(fUpdatable);
    }
    #endregion

    private void Start()
    {
        isStarting = true;
        for (int i = 0; i < starteablesList.Count; i++)
        {
            starteablesList[i].CustomStart();
        }
        isStarting = false;

        FlushStarteablesPending();
    }

    void Update()
    {
        isUpdating = true;
        for (int i = 0; i < updateablesList.Count; i++)
        {
            updateablesList[i].CustomUpdate(Time.deltaTime);
        }
        isUpdating = false;

        FlushUpdateablesPending();
    }

    private void FixedUpdate()
    {
        isFixedUpdating = true;
        for (int i = 0; i < fixedUpdateablesList.Count; i++)
        {
            fixedUpdateablesList[i].CustomFixedUpdate();
        }
        isFixedUpdating = false;

        FlushFixedUpdateablesPending();
    }

    #region Flush de pendientes
    private void FlushStarteablesPending()
    {
        if (starteablesToAdd.Count > 0)
        {
            foreach (var item in starteablesToAdd)
            {
                if (!starteablesList.Contains(item))
                {
                    starteablesList.Add(item);
                }
            }
            starteablesToAdd.Clear();
        }

        if (starteablesToRemove.Count > 0)
        {
            foreach (var item in starteablesToRemove)
            {
                starteablesList.Remove(item);
            }
            starteablesToRemove.Clear();
        }
    }

    private void FlushUpdateablesPending()
    {
        if (updateablesToAdd.Count > 0)
        {
            foreach (var item in updateablesToAdd)
            {
                if (!updateablesList.Contains(item))
                {
                    updateablesList.Add(item);
                }
            }
            updateablesToAdd.Clear();
        }

        if (updateablesToRemove.Count > 0)
        {
            foreach (var item in updateablesToRemove)
            {
                updateablesList.Remove(item);
            }
            updateablesToRemove.Clear();
        }
    }

    private void FlushFixedUpdateablesPending()
    {
        if (fixedUpdateablesToAdd.Count > 0)
        {
            foreach (var item in fixedUpdateablesToAdd)
            {
                if (!fixedUpdateablesList.Contains(item))
                {
                    fixedUpdateablesList.Add(item);
                }
            }
            fixedUpdateablesToAdd.Clear();
        }

        if (fixedUpdateablesToRemove.Count > 0)
        {
            foreach (var item in fixedUpdateablesToRemove)
            {
                fixedUpdateablesList.Remove(item);
            }
            fixedUpdateablesToRemove.Clear();
        }
    }
    #endregion
}