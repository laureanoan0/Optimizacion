using UnityEngine;

public class EnemyDeathVFXManager
{
    private readonly ParticleSystem particleSystem;
    private readonly int burstCount;
    private float minSpeed;
    private float maxSpeed;

    public EnemyDeathVFXManager(ParticleSystem particleSystem, int burstCount = 20, float minSpeed = 2f, float maxSpeed = 6f)
    {
        this.particleSystem = particleSystem;
        this.burstCount = burstCount;
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;

        var main = particleSystem.main;
        main.playOnAwake = false;

        var emission = particleSystem.emission;
        emission.rateOverTime = 0f;
        emission.rateOverDistance = 0f;

        ServiceLocator.Register(this);
    }

    public void PlayAt(Vector3 position, Color color)
    {
        for (int i = 0; i < burstCount; i++) 
        {
            Vector3 randomDirection = Random.onUnitSphere;
            float speed = Random.Range(minSpeed, maxSpeed);

            var emitParams = new ParticleSystem.EmitParams
            {
                position = position,
                startColor = color,
                velocity = randomDirection * speed,
                applyShapeToPosition = true
            };
            particleSystem.Emit(emitParams, 1);
        }
    }
}
