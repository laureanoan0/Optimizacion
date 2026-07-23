using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FastEnemyBehavior : IEnemyBehavior, IUpdateable, IFixedUpdateables
{
    private const float VIEW_HALF_ANGLE = 60f;

    private UnityEngine.Object entity;
    private Transform transform;
    private Transform target;
    private Camera playerCamera;
    private Vector3 originalPosition;
    private float speed;
    private EnemyTypes enemyType = EnemyTypes.fast;
    private int difficulty = 2;
    private bool seen = false;

    public int Difficulty => difficulty;
    public EnemyTypes Type => enemyType;
    public UnityEngine.Object GameObjectRef => entity;

    public event Action<IEnemyBehavior> OnDeath;

    public void Init(UnityEngine.Object entity, Transform playerPos, EnemySO data)
    {
        this.entity = entity;
        transform = entity.GameObject().transform;
        originalPosition = transform.position;
        target = playerPos;
        speed = data.speed;
        playerCamera = Camera.main;
    }

    public void Activate(Vector3 position)
    {
        transform.position = position;
        entity.GameObject().SetActive(true);

        UpdateManager.Instance.Register((IUpdateable)this);
        UpdateManager.Instance.Register((IFixedUpdateables)this);
    }

    public void Deactivate()
    {
        UpdateManager.Instance.Unregister((IUpdateable)this);
        UpdateManager.Instance.Unregister((IFixedUpdateables)this);
        entity.GameObject().SetActive(false);
    }
    public void Destroy()
    {
    }

    public void TakeDamage()
    {
        OnDeath?.Invoke(this);
    }

    public void CustomUpdate(float time)
    {
        if (seen)
        {
            return; 
        }

        var (direction, kill) = EnemySteeringBehavior.Seek(transform, target);

        Vector3 direct = direction * time * speed;
        transform.position += direct;

        if (direct.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(direct);
        }
        if (kill)
        {
            GameplayController.PlayerWon = false;
            GameplayController.LoadFinalScene();
        }
    }
    public void CustomFixedUpdate()
    {
        seen = IsSeenByPlayer();
    }

    private bool IsSeenByPlayer()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null) return false;
        }

        Vector3 toEnemy = transform.position - playerCamera.transform.position;
        float angle = Vector3.Angle(playerCamera.transform.forward, toEnemy);

        return angle <= VIEW_HALF_ANGLE;
    }

    public void Reset()
    {
        transform.position = originalPosition;
    }
}