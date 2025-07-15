
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

public struct FloatRange
{
    public float min;
    public float max;

    public FloatRange(float v1, float v2)
    {
        min = v1;
        max = v2;
    }
};

public struct IntRange
{
    public int min;
    public int max;
    public IntRange(int v1, int v2)
    {
        min = v1;
        max = v2;
    }
};


public class ParticleGenerator
{
    public GeneratorSettings generatorSettings;

    public float mustEmit; // Amount of particles to be emitted within next update call.
    public Vector2 texOffset; // Offset holds half the width and height of the texture.
    public bool isEmitting;
    public List<Particle> particles = new List<Particle>(); // Array of all particles (by pointer).
    public static Shader particleShader;

    // bool (* particle_Deactivator) (
    //   struct Particle *); // Pointer to a function that determines when
    // a particle is deactivated.


    public void InitGenerator(GeneratorSettings _generatorSettings, Image image)
    {
        generatorSettings = _generatorSettings;
        generatorSettings.texture = LoadTextureFromImage(image);
        texOffset.X = generatorSettings.texture.Width / 2;
        texOffset.Y = generatorSettings.texture.Height / 2;
        mustEmit = 0;
        // particleShader = Game.Instance.particleShader;
        // Normalize direction for future uses.
        generatorSettings.direction = ParticleHelper.NormalizeV2(generatorSettings.direction);



        for (int i = 0; i < generatorSettings.capacity; i++)
        {
            Particle p = new Particle();
            p.deactivatorFunction = static (p) =>
            {
                return ParticleHelper.DefaultDeactivator(p);
            };
            particles.Add(p);

        }

    }

    public void ReloadSettings(GeneratorSettings _generatorSettings,Texture2D texture)
    {
        generatorSettings = _generatorSettings;
        generatorSettings.texture = texture;
        texOffset.X = generatorSettings.texture.Width / 2;
        texOffset.Y = generatorSettings.texture.Height / 2;
        mustEmit = 0;
        // particleShader = Game.Instance.particleShader;
        // Normalize direction for future uses.
        generatorSettings.direction = ParticleHelper.NormalizeV2(generatorSettings.direction);



        for (int i = 0; i < generatorSettings.capacity; i++)
        {
            Particle p = new Particle();
            p.deactivatorFunction = static (p) =>
            {
                return ParticleHelper.DefaultDeactivator(p);
            };
            particles.Add(p);

        }
    }

    public void Restart()
    {

    }

    public void Start()
    {
        isEmitting = true;
    }

    public void Stop()
    {
        isEmitting = false;
        UnloadTexture(generatorSettings.texture);
    }

    public void Burst()
    {
        Particle p;
        int emitted = 0;

        int amount = GetRandomValue(generatorSettings.burst.min, generatorSettings.burst.max);

        for (int i = 0; i < generatorSettings.capacity; i++)
        {
            p = particles[i];
            if (!p.active)
            {
                p.Initiate(this);
                p.position = generatorSettings.origin;
                emitted++;
            }
            if (emitted >= amount)
            {
                return;
            }
        }
    }

    public long Update(float deltaTime)
    {
        int emitNow = 0;
        long counter = 0;

        if (isEmitting)
        {
            mustEmit += deltaTime * generatorSettings.emissionRate;
            emitNow = (int)mustEmit; // floor
        }

        for (int i = 0; i < generatorSettings.capacity; i++)
        {
            Particle p = particles[i];
            if (p.active)
            {
                p.Update(p, deltaTime);
                counter++;
            }
            else if (isEmitting && emitNow > 0)
            {
                // emit new particles here
                p.Initiate(this);
                p.Update(p, deltaTime);
                emitNow--;
                mustEmit--;
                counter++;
            }
        }

        return counter;
    }

    public void Draw()
    {
        // BeginShaderMode(particleShader);
        BeginBlendMode(generatorSettings.blendMode);
        for (int i = 0; i < generatorSettings.capacity; i++)
        {
            Particle p = particles[i];
            if (p.active)
            {
                // int particlePos = GetShaderLocation(particleShader, "position");
                // int pColor = GetShaderLocation(particleShader, "color");


                // SetShaderValue(particleShader, particlePos, p.position, ShaderUniformDataType.Vec2);
                // SetShaderValue(particleShader, pColor, ColorLerp(startColor, endColor, p.age / p.ttl), ShaderUniformDataType.Vec4);

                DrawTexture(generatorSettings.texture, (int)(particles[i].position.X - texOffset.X),
                            (int)(particles[i].position.Y - texOffset.Y),
                            ColorLerp(generatorSettings.startColor, generatorSettings.endColor, p.age / p.ttl));
            }
        }
        EndBlendMode();
        // EndShaderMode();
    }
}

