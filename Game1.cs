using System;
using System.Diagnostics;
using DigGame.Draw;
using DigGame.Draw.Camera;
using DigGame.Terrain.Chunking;
using DigGame.Terrain.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Noise;

namespace DigGame;

public class Game1 : Core
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Camera3D camera = new Camera3D()
    {
        Position = new Vector3(-3f, 10f, -3f),
        Pitch = MathF.PI / 2
    };

    private ChunkDrawer chunkDrawer;
    private ChunkManager chunkManager;

    private Texture2D texture;

    private Vector2 halfScreen = new Vector2(16 * 45, 9 * 45);
    
    public Game1() : base("Dig-Game", 16 * 90 , 9 * 90, false)
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

        var stopwatch = Stopwatch.StartNew();

        chunkDrawer = new ChunkDrawer(GraphicsDevice);
        chunkManager = new ChunkManager(chunkDrawer);
        
        chunkManager.LoadChunk(new Coordinate(0, 0, 0, 0));
        chunkManager.LoadChunk(new Coordinate(16, 0, 0, 0));
        chunkManager.LoadChunk(new Coordinate(32, 0, 0, 0));
        chunkManager.LoadChunk(new Coordinate(0, 0, 16, 0));
        chunkManager.LoadChunk(new Coordinate(16, 0, 16, 0));
        chunkManager.LoadChunk(new Coordinate(32, 0, 16, 0));
        chunkManager.LoadChunk(new Coordinate(0, 0, 32, 0));
        chunkManager.LoadChunk(new Coordinate(16, 0, 32, 0));
        chunkManager.LoadChunk(new Coordinate(32, 0, 32, 0));

        chunkManager.UnloadChunk(new Coordinate(16, 0, 16, 0));
        
        stopwatch.Stop();
        
        Console.WriteLine($"took {Math.Round(stopwatch.ElapsedTicks * (1000000f / Stopwatch.Frequency), 1)} microseconds");
    }

    protected override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        camera.Yaw += (halfScreen.X - mouseState.X) * 0.005f;
        camera.Pitch += (halfScreen.Y - mouseState.Y) * 0.005f;
        
        Mouse.SetPosition((int)halfScreen.X, (int)halfScreen.Y);
        
        var forwardVector = camera.PitchYawToVector3(-camera.Pitch, camera.Yaw);
        var rightVector = camera.PitchYawToVector3(0, camera.Yaw + MathF.PI / 2);
        
        var keyboardState = Keyboard.GetState();
        
        if (keyboardState.IsKeyDown(Keys.Escape)) Exit();
        
        if (keyboardState.IsKeyDown(Keys.A))
        {
            camera.Position -= rightVector * 0.1f;
        }

        if (keyboardState.IsKeyDown(Keys.D))
        {
            camera.Position += rightVector * 0.1f;
        }
        
        if (keyboardState.IsKeyDown(Keys.S))
        {
            camera.Position -= forwardVector * 0.1f;
        }

        if (keyboardState.IsKeyDown(Keys.W))
        {
            camera.Position += forwardVector * 0.1f;
        }
        
        camera.shader.View = Matrix.CreateLookAt(camera.Position, camera.Position + forwardVector, Vector3.Up);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        chunkDrawer.DrawUploadedChunks(camera.shader);

        base.Draw(gameTime);
    }
}