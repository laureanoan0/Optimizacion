using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class BehaviorFactory
{
    public static IEntityBehavior CreateEntityBehavior(EntityType type, Transform orientation, Transform transform, Transform playerT, Rigidbody rb, PlayerStatsSO stats)
    {
        switch (type)
        {
            case EntityType.Camera: return new CameraBehavior(orientation, transform, playerT);
            case EntityType.Player: return new PlayerBehavior(orientation, transform, rb, stats);
            case EntityType.Enemy: return new CameraBehavior(orientation, transform, playerT);
            default: return null;
        }
    }
}

