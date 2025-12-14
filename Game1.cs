using System;
using DigGame.Draw.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace DigGame;

public class Game1 : Core
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Camera3D camera = new Camera3D()
    {
        Position = new Vector3(-3f, 5f, -3f)
    };

    private VertexBuffer vertexBuffer;

    private Texture2D texture;
    
    public Game1() : base("Dig-Game", 1920, 1080, true)
    {
        
    }

    protected override void Initialize()
    {
        Graphics.PreferMultiSampling = false;
        Graphics.ApplyChanges();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        texture = Content.Load<Texture2D>("Texture/textureSheet");
        
        camera.shader = new BasicEffect(GraphicsDevice)
        {
            VertexColorEnabled = true,
            World = Matrix.Identity,
            View = Matrix.CreateLookAt(camera.Position, Vector3.Zero, Vector3.Right),
            Projection = Matrix.CreatePerspectiveFieldOfView(float.DegreesToRadians(90f), 16f / 9f, 0.1f, 100.0f),
            Texture = texture
        };
        camera.shader.TextureEnabled = true;
        camera.shader.VertexColorEnabled = false; // do not remove this line

        GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        
        vertexBuffer = new VertexBuffer(
            GraphicsDevice,
            VertexPositionTexture.VertexDeclaration,
            36,
            BufferUsage.None
        );
        
        VertexPositionTexture[] vertices = new[]
        {
            new VertexPositionTexture(new Vector3(-1.5f, -1.5f, -1.5f), new Vector2(0, 1f)),
            new VertexPositionTexture(new Vector3(-1.5f, 1.5f, -1.5f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(-1.5f, -1.5f, 1.5f), new Vector2(1f, 1f)),
            new VertexPositionTexture(new Vector3(-1.5f, -1.5f, 1.5f), new Vector2(1f, 1f)),
            new VertexPositionTexture(new Vector3(-1.5f, 1.5f, -1.5f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(-1.5f, 1.5f, 1.5f), new Vector2(1f, 0)),
            new VertexPositionTexture(new Vector3(-1.5f, -1.5f, -1.5f), new Vector2(0, 1f)),
            new VertexPositionTexture(new Vector3(1.5f, -1.5f, -1.5f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(-1.5f, 1.5f, -1.5f), new Vector2(1f, 1f)),
            new VertexPositionTexture(new Vector3(1.5f, -1.5f, -1.5f), new Vector2(0f, 1f)),
            new VertexPositionTexture(new Vector3(1.5f, 1.5f, -1.5f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(-1.5f, 1.5f, -1.5f), new Vector2(1f, 0)),
            new VertexPositionTexture(new Vector3(-1.5f, 1.5f, -1.5f), new Vector2(0, 1f)),
            new VertexPositionTexture(new Vector3(1.5f, 1.5f, -1.5f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(-1.5f, 1.5f, 1.5f), new Vector2(1f, 1f)),
            new VertexPositionTexture(new Vector3(-1.5f, 1.5f, 1.5f), new Vector2(1f, 1f)),
            new VertexPositionTexture(new Vector3(1.5f, 1.5f, -1.5f), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1.5f, 1.5f, 1.5f), new Vector2(1f, 0)),
        };
        
        vertexBuffer.SetData(0, vertices, 0, 18, 0 );
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        var keyboardState = Keyboard.GetState();
        
        if (keyboardState.IsKeyDown(Keys.A))
        {
            camera.Position += new Vector3(0f, 0f, -0.1f);
        }

        if (keyboardState.IsKeyDown(Keys.D))
        {
            camera.Position += new Vector3(0f, 0f, 0.1f);
        }
        
        if (keyboardState.IsKeyDown(Keys.S))
        {
            camera.Position += new Vector3(-0.1f, 0f, 0f);
        }

        if (keyboardState.IsKeyDown(Keys.W))
        {
            camera.Position += new Vector3(0.1f, 0f, 0f);
        }
        
        camera.shader.View = Matrix.CreateLookAt(camera.Position, Vector3.Zero, Vector3.Up);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        camera.shader.Texture = texture;
        
        foreach (var pass in camera.shader.CurrentTechnique.Passes)
        {
            pass.Apply();
            
            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 6);
        }

        base.Draw(gameTime);
    }
}