using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

// struct Particle
// {
//     public Vector2 position;
//     public Vector2 velocity;

//     public Color color;
//     public float lifeTime;

//     public Particle()
//     {
//         position = new Vector2(0);
//         velocity = new Vector2(0);
//         color = Color.White;
//         lifeTime = 0f;
//     }
// }


public class Particle
{
    public Vector2 origin;               // The origin of the particle (never changes).
    public Vector2 position;             // Position of the particle in 2d space.
    public Vector2 velocity;             // Velocity vector in 2d space.
    public Vector2 externalAcceleration; // Acceleration vector in 2d space.
    public float originAcceleration;     // Accelerates velocity vector
    public float age;                    // Age is measured in seconds.
    public float ttl;                    // Ttl is the time to live in seconds.
    public bool active; // Inactive particles are neither updated nor drawn.

    public Func<Particle, bool> deactivatorFunction; // Pointer to a function that determines
                                             // when a particle is deactivated.

    public void Initiate(ParticleGenerator particleGenerator)
    {
        age = 0;
        origin = particleGenerator.generatorSettings.origin;

        // Get a random angle to find an random velocity.
        float randa =
            (float)MiscHelpers.GetRandomNumber(particleGenerator.generatorSettings.directionAngle.min, particleGenerator.generatorSettings.directionAngle.max);

        // Rotate base direction with the given angle.
        Vector2 res = ParticleHelper.RotateV2(particleGenerator.generatorSettings.direction, randa);

        // Get a random value for velocity range (direction is normalized).
        float randv =  (float)MiscHelpers.GetRandomNumber(particleGenerator.generatorSettings.velocity.min, particleGenerator.generatorSettings.velocity.max); 

        // Multiply direction with factor to set actual velocity in the Particle.
        velocity = new Vector2(x: res.X * randv, y: res.Y * randv);

        // Get a random angle to rotate the velocity vector.
        randa = (float)MiscHelpers.GetRandomNumber(particleGenerator.generatorSettings.velocityAngle.min, particleGenerator.generatorSettings.velocityAngle.max);

        // Rotate velocity vector with given angle.
        velocity = ParticleHelper.RotateV2(velocity, randa);

        // Get a random value for origin offset and apply it to position.
        float rando = (float)MiscHelpers.GetRandomNumber(particleGenerator.generatorSettings.offset.min, particleGenerator.generatorSettings.offset.max);
        position.X = particleGenerator.generatorSettings.origin.X + res.X * rando;
        position.Y = particleGenerator.generatorSettings.origin.Y + res.Y * rando;

        // Get a random value for the intrinsic particle acceleration
        float rands =
            (float)MiscHelpers.GetRandomNumber(particleGenerator.generatorSettings.originAcceleration.min, particleGenerator.generatorSettings.originAcceleration.max);
        originAcceleration = rands;
        externalAcceleration = particleGenerator.generatorSettings.externalAcceleration;
        ttl = (float)MiscHelpers.GetRandomNumber(particleGenerator.generatorSettings.age.min, particleGenerator.generatorSettings.age.max);
        active = true;
    }

    public void Update(Particle p, float dt)
    {
        if (!active)
        {
            return;
        }

        age += dt;

        if (deactivatorFunction(p))
        {
            active = false;
            return;
        }

        Vector2 toOrigin = ParticleHelper.NormalizeV2(new Vector2(x: origin.X - position.X,
                                           y: origin.Y - position.Y));

        // Update velocity by internal acceleration.
        velocity.X += toOrigin.X * originAcceleration * dt;
        velocity.Y += toOrigin.Y * originAcceleration * dt;

        // Update velocity by eXternal acceleration.
        velocity.X += externalAcceleration.X * dt;
        velocity.Y += externalAcceleration.Y * dt;

        // Update position by velocity.
        position.X += velocity.X * dt;
        position.Y += velocity.Y * dt;
    }
};