﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monogame_demo
{
  public class Player
  {
    public Vector2 velocity = Vector2.Zero;
    public Vector2 position = Vector2.Zero;
    public Vector2 lastPosition = Vector2.Zero;
    public Vector2 direction = new Vector2(0, 0);
    public float rotation = 0f;
  }

  public class Game1 : Game
  {

    const int TargetWidth = 480; // 384
    const int TargetHeight = 270; // 216
    Matrix Scale;

    float scaleX = 0;
    float scaleY = 0;

    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    SimpleFps fps = new SimpleFps();
    SpriteFont font;

    private Texture2D playerTexture;
    private Texture2D mapTexture;

    private Player player = new Player();

    private double last = 0;

    private int gameScale = 3;

    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;

      graphics.PreferredBackBufferWidth = 1920;
      graphics.PreferredBackBufferHeight = 1080;
      graphics.IsFullScreen = true;
      graphics.PreferMultiSampling = false;
      graphics.SynchronizeWithVerticalRetrace = true;
      graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
      scaleX = graphics.PreferredBackBufferWidth / TargetWidth;
      scaleY = graphics.PreferredBackBufferHeight / TargetHeight;

      Scale = Matrix.CreateScale(new Vector3(scaleX, scaleY, 1));

      base.Initialize();
    }

    protected override void LoadContent()
    {
      spriteBatch = new SpriteBatch(GraphicsDevice);

      font = Content.Load<SpriteFont>("PressStart2P");
      playerTexture = Content.Load<Texture2D>("megaman-left");
      mapTexture = Content.Load<Texture2D>("map");

      player.position.X = 20;
      player.position.Y = TargetHeight / 2 - playerTexture.Height / 2;
      player.velocity.X = 60;
      player.velocity.Y = 60;
    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      player.direction.X = 0;
      player.direction.Y = 0;

      if (Keyboard.GetState().IsKeyDown(Keys.Left))
      {
        player.direction.X = -1;
      }
      else if (Keyboard.GetState().IsKeyDown(Keys.Right))
      {
        player.direction.X = 1;
      }

      if (Keyboard.GetState().IsKeyDown(Keys.Up))
      {
        player.direction.Y = -1;
      }
      else if (Keyboard.GetState().IsKeyDown(Keys.Down))
      {
        player.direction.Y = 1;
      }

      fps.Update(gameTime);

      var now = gameTime.TotalGameTime.TotalSeconds;
      var elapsed = now - last;
      var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

      player.lastPosition.X = player.position.X;
      player.lastPosition.Y = player.position.Y;
      player.position.X += player.velocity.X * delta * player.direction.X;
      player.position.Y += player.velocity.Y * delta * player.direction.Y;

      last = now;

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

      GraphicsDevice.Clear(Color.Gray);

      // spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
      spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, Scale);

      // Draw the fps msg
      // fps.DrawFps(spriteBatch, font, new Vector2(10f, 10f), Color.White);

      spriteBatch.Draw(mapTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

      spriteBatch.Draw(playerTexture, player.position, null, Color.White, player.rotation, new Vector2(12, 12), 1f, SpriteEffects.None, 0f);

      spriteBatch.DrawString(font, frameRate.ToString(), new Vector2(10f, 10f), Color.White);

      spriteBatch.End();


      base.Draw(gameTime);
    }
  }

  public class SimpleFps
  {
    private double frames = 0;
    private double updates = 0;
    private double elapsed = 0;
    private double last = 0;
    private double now = 0;
    public double msgFrequency = 1.0f;
    public string msg = "";

    /// <summary>
    /// The msgFrequency here is the reporting time to update the message.
    /// </summary>
    public void Update(GameTime gameTime)
    {
      now = gameTime.TotalGameTime.TotalSeconds;
      elapsed = (double)(now - last);

      if (elapsed > msgFrequency)
      {
        msg = " Fps: " + (frames / elapsed).ToString() + "\n Elapsed time: " + elapsed.ToString() + "\n Updates: " + updates.ToString() + "\n Frames: " + frames.ToString();
        elapsed = 0;
        frames = 0;
        updates = 0;
        last = now;
      }

      updates++;
    }

    public void DrawFps(SpriteBatch spriteBatch, SpriteFont font, Vector2 fpsDisplayPosition, Color fpsTextColor)
    {
      spriteBatch.DrawString(font, msg, fpsDisplayPosition, fpsTextColor);
      frames++;
    }
  }
}
