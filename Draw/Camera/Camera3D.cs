using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigGame.Draw.Camera;

public class Camera3D
{
    public BasicEffect shader { get; set; }

    public Vector3 Position { get; set; }
    
    public float Pitch { get; set; }
    public float Yaw { get; set; }

    public Vector3 PitchYawToVector3(float pitch, float yaw)
    {
        float cosPitch = MathF.Cos(pitch);

        return new Vector3(
            cosPitch * MathF.Sin(yaw),
            MathF.Sin(pitch),
            cosPitch * MathF.Cos(yaw)
        );
    }
    
    public Camera3D(){}
}