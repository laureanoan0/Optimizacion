using UnityEngine;

public class EnemyDeathVFXManager
{
    private readonly ParticleSystem particleSystem;
    private readonly int burstCount;

    public EnemyDeathVFXManager(ParticleSystem particleSystem, int burstCount = 20)
    {
        this.particleSystem = particleSystem;
        this.burstCount = burstCount;

        var main = particleSystem.main;
        main.playOnAwake = false;

        var emission = particleSystem.emission;
        emission.rateOverTime = 0f;
        emission.rateOverDistance = 0f;

        ServiceLocator.Register(this);
    }

    public void PlayAt(Vector3 position)
    {
        PlayAt(position, Color.white);
    }

    public void PlayAt(Vector3 position, Color color)
    {
        var emitParams = new ParticleSystem.EmitParams
        {
            position = position,
            startColor = color
        };
        particleSystem.Emit(emitParams, burstCount);
    }
}
