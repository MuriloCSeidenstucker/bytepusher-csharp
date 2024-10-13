using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BytePusherCsharp.src;

public class BytePusherMg : Game
{
    private GraphicsDeviceManager? graphics;
    private SpriteBatch? spriteBatch;
    private BytePusherVM? vm;
    private MgBytepusherIODriver? driver;

    public BytePusherMg()
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.IsFullScreen = false;
        graphics.PreferredBackBufferHeight = 768;
        graphics.PreferredBackBufferWidth = 1024;
        Content.RootDirectory = "Content";
        this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60);
    }

    protected override void LoadContent()
    {
        base.LoadContent();

        // Initialize BytePusherVM
        driver = new MgBytepusherIODriver(GraphicsDevice);
        vm = new BytePusherVM(driver);
        vm.Load(@"C:\Users\mseid\Downloads\Sprites.BytePusher");

        // Setup fonts and screen dimensions
        spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    // Update called every 60th second. This seems to  work fine up to 60fps
    protected override void Update(GameTime gameTime)
    {
        vm?.Run();
        base.Update(gameTime);
    }

    // Called once per updated(), however calls may be dropped
    protected override void Draw(GameTime gameTime)
    {
        graphics?.GraphicsDevice.Clear(Color.Black);
        spriteBatch?.Begin();

        // Render BytePusherVM display
        spriteBatch?.Draw(driver?.Texture, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
        spriteBatch?.End();
        base.Draw(gameTime);
    }
}