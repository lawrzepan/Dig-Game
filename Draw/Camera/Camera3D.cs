using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigGame.Draw.Camera;

public class Camera3D
{
    public BasicEffect shader { get; set; }

    public Vector3 Position { get; set; }
    
    public Camera3D(){}
}