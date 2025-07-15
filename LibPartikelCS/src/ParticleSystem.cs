
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

public static class ParticleSystem
{

    public static bool active;
    public static int capacity;
    public static Vector2 origin;
    public static List<ParticleGenerator> particleGenerators;

    public static void Initiate()
    {
        particleGenerators = new List<ParticleGenerator>();
        active = false;
        capacity = 10;
        origin = new Vector2(Game.Instance.screenWidth / 2 ,Game.Instance.screenHeight / 2 );
    }
    public static void Start()
    {
        for (int i = 0; i < particleGenerators.Count; i++)
        {
            particleGenerators[i].Start();
        }
    }


    public static bool AddToRegister(ParticleGenerator particleGenerator)
    {

        if (particleGenerators.Count >= capacity)
        {
            return false;
        }
        particleGenerators.Add(particleGenerator);

        return true;
    }


    public static bool RemoveFromRegister(ParticleGenerator particleGenerator)
    {
        int index = particleGenerators.IndexOf(particleGenerator);
        if (index == -1)
        {
            return false;
        }
        particleGenerators.RemoveAt(index);
        return true;
    }

    public static void Stop()
    {
        for (int i = 0; i < particleGenerators.Count; i++)
        {
            particleGenerators[i].Stop();
        }
    }

    public static void BurstAllGenerators()
    {
        for (int i = 0; i < particleGenerators.Count; i++)
        {
            particleGenerators[i].Burst();
        }
    }

    public static void Draw()
    {
        for (int i = 0; i < particleGenerators.Count; i++)
        {
            particleGenerators[i].Draw();
        }
    }

    public static long Update(float deltaTime)
    {
        long counter = 0;
        for (int i = 0; i < particleGenerators.Count; i++)
        {
            if (particleGenerators[i] != null)
            {
                if (particleGenerators[i].particles.Count > 0)
                {
                    // Console.WriteLine($"Updating particle gen: {i}");
                    counter += particleGenerators[i].Update(deltaTime);

                }
            }

        }
        return counter;
    }



}