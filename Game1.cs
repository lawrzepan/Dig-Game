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

    private VertexBuffer OnScreenBuffer;
    private BasicEffect onScreenEffect;

    private long tick = 0;
    
    public Game1() : base("Dig-Game", 16 * 90 , 9 * 90, false)
    {
        Window.IsBorderless = false;
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
            Projection = Matrix.CreatePerspectiveFieldOfView(float.DegreesToRadians(90f), 16f / 9f, 0.1f, 3000.0f),
            Texture = texture
        };
        camera.shader.TextureEnabled = true;
        camera.shader.VertexColorEnabled = false; // do not remove this line

        GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

        var stopwatch = Stopwatch.StartNew();

        chunkDrawer = new ChunkDrawer(GraphicsDevice);
        chunkManager = new ChunkManager(chunkDrawer);
        
        stopwatch.Stop();
        
        Console.WriteLine($"took {Math.Round(stopwatch.ElapsedTicks * (1000000f / Stopwatch.Frequency), 1)} microseconds");

        OnScreenBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 50000,
            BufferUsage.WriteOnly);
        onScreenEffect = new BasicEffect(GraphicsDevice)
        {
            World = Matrix.Identity,
            View = Matrix.Identity,
            Projection = Matrix.CreateOrthographicOffCenter(
                0, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, 0, // Invert Y-axis for top-left origin
                0, 1 // Near and Far planes
            ),
            VertexColorEnabled = true
        };
    }

    protected override void Update(GameTime gameTime)
    {
        tick += 1;
        
        var mouseState = Mouse.GetState();

        camera.Yaw += (halfScreen.X - mouseState.X) * 0.005f;
        camera.Pitch += (halfScreen.Y - mouseState.Y) * 0.005f;
        
        Mouse.SetPosition((int)halfScreen.X, (int)halfScreen.Y);
        
        var forwardVector = camera.PitchYawToVector3(-camera.Pitch, camera.Yaw);
        var rightVector = camera.PitchYawToVector3(0, camera.Yaw + MathF.PI / 2);
        
        var keyboardState = Keyboard.GetState();
        
        if (keyboardState.IsKeyDown(Keys.Escape)) Exit();

        float speed = 0.1f;

        if (keyboardState.IsKeyDown(Keys.LeftShift))
        {
            speed = 0.5f;
        }
        
        if (keyboardState.IsKeyDown(Keys.A))
        {
            camera.Position -= rightVector * speed;
        }

        if (keyboardState.IsKeyDown(Keys.D))
        {
            camera.Position += rightVector * speed;
        }
        
        if (keyboardState.IsKeyDown(Keys.S))
        {
            camera.Position -= forwardVector * speed;
        }

        if (keyboardState.IsKeyDown(Keys.W))
        {
            camera.Position += forwardVector * speed;
        }
        
        camera.shader.View = Matrix.CreateLookAt(camera.Position, camera.Position + forwardVector, Vector3.Up);

        if (tick % 3 == 0)
        {
            chunkManager.LoadChunksInRadius(64f, new Coordinate(
                (int)Math.Floor(camera.Position.X / (float)Chunk.Size) * Chunk.Size, 0,
                (int)Math.Floor(camera.Position.Z / (float)Chunk.Size) * Chunk.Size, 0));
        }
        
        Vector2 diagramSize = new Vector2(75, 710);
        Vector2 topLeft = new Vector2(1315, 50);
        
        VertexPositionColor[] pagerDiagram = new VertexPositionColor[50000];

        var pager = chunkManager.ChunkDrawer.vertexPager;
        
        int i = 0;
        for (int range = 0; range < pager.Ranges.Count; range++)
        {
            if (range >= pager.Ranges.Count) break;
            var currentRange = pager.Ranges[range];
            
            if (currentRange.RangeType == RangeType.Full)
            {
                pagerDiagram[i] = new VertexPositionColor(new Vector3(topLeft.X, 
                    (float)currentRange.Start / pager.ArrayLength * diagramSize.Y + topLeft.Y, 
                    0), Color.Green);
                pagerDiagram[i + 1] = new VertexPositionColor(new Vector3(topLeft.X + diagramSize.X, 
                    (float)currentRange.Start / pager.ArrayLength * diagramSize.Y + topLeft.Y, 
                    0), Color.Green);
                pagerDiagram[i + 2] = new VertexPositionColor(new Vector3(topLeft.X, 
                    (float)currentRange.End / pager.ArrayLength * diagramSize.Y + topLeft.Y, 
                    0), Color.Green);
                pagerDiagram[i + 3] = new VertexPositionColor(new Vector3(topLeft.X, 
                    (float)currentRange.End / pager.ArrayLength * diagramSize.Y + topLeft.Y, 
                    0), Color.Green);
                pagerDiagram[i + 4] = new VertexPositionColor(new Vector3(topLeft.X + diagramSize.X, 
                    (float)currentRange.Start / pager.ArrayLength * diagramSize.Y + topLeft.Y, 
                    0), Color.Green);
                pagerDiagram[i + 5] = new VertexPositionColor(new Vector3(topLeft.X + diagramSize.X, 
                    (float)currentRange.End / pager.ArrayLength * diagramSize.Y + topLeft.Y, 
                    0), Color.Green);
                i += 6;
            }
        }

        Vector2 verticesDrawnLocation = new Vector2(topLeft.X - 20,
            pager.GetNumVerticesToDraw() / (float)pager.ArrayLength + topLeft.Y - 0.5f);

        pagerDiagram[i] = new VertexPositionColor(new Vector3(verticesDrawnLocation, 0), Color.White);
        pagerDiagram[i + 1] = new VertexPositionColor(new Vector3(verticesDrawnLocation + new Vector2(15, 0), 0), Color.White);
        pagerDiagram[i + 2] = new VertexPositionColor(new Vector3(verticesDrawnLocation + new Vector2(0, 1), 0), Color.White);
        pagerDiagram[i + 3] = new VertexPositionColor(new Vector3(verticesDrawnLocation + new Vector2(0, 1), 0), Color.White);
        pagerDiagram[i + 4] = new VertexPositionColor(new Vector3(verticesDrawnLocation + new Vector2(15, 0), 0), Color.White);
        pagerDiagram[i + 5] = new VertexPositionColor(new Vector3(verticesDrawnLocation + new Vector2(15, 1), 0), Color.White);
        i += 6;
        
        OnScreenBuffer.SetData(0, pagerDiagram, 0, pagerDiagram.Length, 0);
        
        base.Update(gameTime);
        
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        chunkDrawer.DrawUploadedChunks(camera.shader);

        foreach (var pass in onScreenEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            GraphicsDevice.SetVertexBuffer(OnScreenBuffer);
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, (int)MathF.Floor(OnScreenBuffer.VertexCount / 3f));
        }

        base.Draw(gameTime);
    }
}