using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monogame_demo
{
  public class Bunny
  {
    public Vector2 velocity = Vector2.Zero;
    public Vector2 position = Vector2.Zero;
    public Vector2 lastPosition = Vector2.Zero;
    public Vector2 direction = new Vector2(0, 0);
    public float rotation = 0f;
  }

  public class Game1 : Game
  {

    const int TargetWidth = 384;
    const int TargetHeight = 216;
    Matrix Scale;

    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    SimpleFps fps = new SimpleFps();
    SpriteFont font;

    private Texture2D bunnyTexture;

    private Bunny bunny = new Bunny();

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
      // TODO: Add your initialization logic here

      float scaleX = graphics.PreferredBackBufferWidth / TargetWidth;
      float scaleY = graphics.PreferredBackBufferHeight / TargetHeight;
      Scale = Matrix.CreateScale(new Vector3(scaleX, scaleY, 1));

      base.Initialize();
    }

    protected override void LoadContent()
    {
      spriteBatch = new SpriteBatch(GraphicsDevice);

      // TODO: use this.Content to load your game content here
      font = Content.Load<SpriteFont>("PressStart2P");
      bunnyTexture = Content.Load<Texture2D>("bunny");

      bunny.position.X = 20;
      bunny.position.Y = TargetHeight / 2 - bunnyTexture.Height / 2;
      bunny.velocity.X = 60;
      bunny.velocity.Y = 60;
    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      bunny.direction.X = 0;
      bunny.direction.Y = 0;

      if (Keyboard.GetState().IsKeyDown(Keys.Left))
      {
        bunny.direction.X = -1;
      }
      else if (Keyboard.GetState().IsKeyDown(Keys.Right))
      {
        bunny.direction.X = 1;
      }

      if (Keyboard.GetState().IsKeyDown(Keys.Up))
      {
        bunny.direction.Y = -1;
      }
      else if (Keyboard.GetState().IsKeyDown(Keys.Down))
      {
        bunny.direction.Y = 1;
      }


      // TODO: Add your update logic here
      fps.Update(gameTime);

      var now = gameTime.TotalGameTime.TotalSeconds;
      var elapsed = now - last;
      var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

      bunny.lastPosition.X = bunny.position.X;
      bunny.lastPosition.Y = bunny.position.Y;
      bunny.position.X += (bunny.velocity.X * delta) * bunny.direction.X;
      bunny.position.Y += (bunny.velocity.Y * delta) * bunny.direction.Y;

      last = now;

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

      GraphicsDevice.Clear(Color.Gray);

      // TODO: Add your drawing code here
      // spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
      spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, Scale);

      // Draw the fps msg
      // fps.DrawFps(spriteBatch, font, new Vector2(10f, 10f), Color.White);

      // spriteBatch.DrawString(font, frameRate.ToString(), new Vector2(10f, 200f), Color.White);

      spriteBatch.Draw(bunnyTexture, bunny.position, null, Color.White, bunny.rotation, new Vector2(13, 18.5f), 1f, SpriteEffects.None, 0f);

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
