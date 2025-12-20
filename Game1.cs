using System;
using System.Diagnostics;
using DigGame.Draw;
using DigGame.Draw.Camera;
using DigGame.Terrain.Chunking;
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
        Position = new Vector3(-3f, 5f, -3f)
    };

    private ChunkDrawer chunkDrawer;
    private Chunk chunk;

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

        var stopwatch = Stopwatch.StartNew();

        texture = new Texture2D(GraphicsDevice, 128, 128);

        Color[] data = new Color[128 * 128];
        for (float x = 0; x < 128f; x++)
        {
            for (float y = 0; y < 128f; y++)
            {
                float intensity = (float)Noise.Perlin(x / 10, y / 10) / 2f + 0.5f;
                data[(int)x + (int)y * 128] = new Color(intensity, intensity, intensity);
            }
        }
        texture.SetData(data);
        camera.shader.Texture = texture;
        
        chunk = new Chunk();
        chunkDrawer = new ChunkDrawer(GraphicsDevice);
        chunkDrawer.UploadChunk(chunk);
        
        stopwatch.Stop();
        
        Console.WriteLine($"took {Math.Round(stopwatch.ElapsedTicks * (1000000f / Stopwatch.Frequency), 1)} microseconds");
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        
        if (keyboardState.IsKeyDown(Keys.Escape)) Exit();
        
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
        GraphicsDevice.Clear(Color.Black);
        
        chunkDrawer.DrawUploadedChunks(camera.shader);

        base.Draw(gameTime);
    }
}