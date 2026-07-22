using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LineRendererPool : IUpdateable
{
    private class ActiveLine
    {
        public LineRenderer LineRend;
        public Vector3 StartPos;
        public Vector3 EndPos;
        public float Timer;
        public float Duration;
    }

    private readonly LineRenderer lineRendPrefab;
    private readonly ObjectPool<LineRenderer> pool;
    private readonly List<ActiveLine> activeLines = new List<ActiveLine>();
    private readonly float shootLifetime;

    public LineRendererPool(LineRenderer lineRendPrefab, int prewarmCount = 5, float shootLifetime = 0.10f)
    {
        this.lineRendPrefab = lineRendPrefab;
        this.shootLifetime = shootLifetime;
        pool = CreatePool(prewarmCount);
        UpdateManager.Instance.Register(this);
    }

    public void Destroy()
    {
        UpdateManager.Instance.Unregister(this);
    }

    public void CustomUpdate(float time)
    {
        for (int i = activeLines.Count - 1; i >= 0; i--)
        {
            var line = activeLines[i];
            line.Timer += time;

            if (line.Timer >= line.Duration)
            {
                pool.Release(line.LineRend);
                activeLines.RemoveAt(i);
            }
        }
    }

    public void Shoot(Vector3 startPos, Vector3 endPos)
    {
        LineRenderer lineRend = pool.Get();
        lineRend.SetPosition(0, startPos);
        lineRend.SetPosition(1, endPos);

        activeLines.Add(new ActiveLine
        {
            LineRend = lineRend,
            StartPos = startPos,
            EndPos = endPos,
            Timer = 0f,
            Duration = shootLifetime
        });
    }

    private ObjectPool<LineRenderer> CreatePool(int prewarmCount)
    {
        ObjectPool<LineRenderer> newPool = new ObjectPool<LineRenderer>(
            createFunc: CreateLineInstance,
            actionOnGet: OnGetLine,
            actionOnRelease: OnReleaseLine,
            actionOnDestroy: OnDestroyLine,
            collectionCheck: false,
            defaultCapacity: prewarmCount,
            maxSize: 10);

        PrewarmPool(newPool, prewarmCount);

        return newPool;
    }

    private void PrewarmPool(ObjectPool<LineRenderer> targetPool, int prewarmCount)
    {
        var warmBatch = new List<LineRenderer>(prewarmCount);
        for (int i = 0; i < prewarmCount; i++)
        {
            warmBatch.Add(targetPool.Get());
        }
        foreach (var line in warmBatch)
        {
            targetPool.Release(line);
        }
    }

    private LineRenderer CreateLineInstance()
    {
        LineRenderer instance = Object.Instantiate(lineRendPrefab);
        instance.positionCount = 2;
        instance.gameObject.SetActive(false);
        return instance;
    }

    private void OnGetLine(LineRenderer line)
    {
        line.gameObject.SetActive(true);
    }

    private void OnReleaseLine(LineRenderer line)
    {
        line.gameObject.SetActive(false);
    }

    private void OnDestroyLine(LineRenderer line)
    {
        Object.Destroy(line.gameObject);
    }
}