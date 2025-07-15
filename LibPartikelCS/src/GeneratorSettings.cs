
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;


public struct GeneratorSettings
{
    public Vector2 direction;         // Direction vector will be normalized.
    public FloatRange velocity;       // The possible range of the particle velocities.
                                      // Velocity is a scalar defining the length of the
                                      // direction vector.
    public FloatRange directionAngle; // The angle range modiying the direction vector.
    public FloatRange velocityAngle;  // The angle range to rotate the velocity vector.
    public FloatRange offset; // The min and max offset multiplier for the particle origin.
    public FloatRange originAcceleration; // An acceleration towards or from
                                          // (centrifugal) the origin.
    public IntRange burst;                // The range of sudden particle bursts.
    public int capacity;               // Maximum amounts of particles in the system.
    public int emissionRate;           // Rate of emitted particles per second.
    public Vector2 origin;                // Origin is the source of the emitter.
    public Vector2 externalAcceleration; // External constant acceleration. e.g. gravity.
    public Color startColor;    // The color the particle starts with when it spawns.
    public Color endColor;      // The color the particle ends with when it disappears.
    public FloatRange age;      // Age range of particles in seconds.
    public BlendMode blendMode; // Color blending mode for all particles of this Emitter.
    public Texture2D texture;   // The texture used as particle texture.

    public Func<Particle, bool> deactivatorFunction;
}