using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-100)]
public class UpdateManager : MonoBehaviour
{
    private static UpdateManager instance;
    public static UpdateManager Instance => instance;

    private readonly List<IUpdateable> updateablesList = new List<IUpdateable>();
    private readonly List<IFixedUpdateables> fixedUpdateablesList = new List<IFixedUpdateables>();
 
    private readonly List<IUpdateable> updateablesToAdd = new List<IUpdateable>();
    private readonly List<IUpdateable> updateablesToRemove = new List<IUpdateable>();
    private readonly List<IFixedUpdateables> fixedUpdateablesToAdd = new List<IFixedUpdateables>();
    private readonly List<IFixedUpdateables> fixedUpdateablesToRemove = new List<IFixedUpdateables>();

    private bool isUpdating;
    private bool isFixedUpdating;
    private bool isPaused;
    public bool IsPaused => isPaused;

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

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
    }

    #region Registros

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

    void Update()
    {
        if (isPaused) return;

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
        if (isPaused) return;

        isFixedUpdating = true;
        for (int i = 0; i < fixedUpdateablesList.Count; i++)
        {
            fixedUpdateablesList[i].CustomFixedUpdate();
        }
        isFixedUpdating = false;

        FlushFixedUpdateablesPending();
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }

    #region Flush de pendientes

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