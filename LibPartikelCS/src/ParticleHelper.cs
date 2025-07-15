using System.Numerics;

public static class ParticleHelper
{

    public static Vector2 NormalizeV2(Vector2 v)
    {
        if (v.X == 0 && v.Y == 0)
        {
            return v;
        }
        float len = MathF.Sqrt(v.X * v.X + v.Y * v.Y);
        return new Vector2(x: v.X / len, y: v.Y / len);
    }

    public static Vector2 RotateV2(Vector2 v, float degrees)
    {
        float rad = float.DegreesToRadians(degrees);
        Vector2 res = new Vector2(x: MathF.Cos(rad) * v.X - MathF.Sin(rad) * v.Y,
                 y: MathF.Sin(rad) * v.X + MathF.Cos(rad) * v.Y);
        return res;
    }

    public static bool DefaultDeactivator(Particle p)
    {
        return p.age > p.ttl;
    }
}