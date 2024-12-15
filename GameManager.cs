using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonGame.Assets;
using MonGame.Drawing;
using MonGame.ECS;
using MonGame.Input;
using MonGame.Sound;
using MonGame.UI;
using MonGame.World2D;
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace MonGame
{
    public class GameManager : Game
    {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public Ecs Ecs;

        public Texture2DAsset Birb;
        public Texture2DAsset Birb2;
        public Texture2DAsset Birb3;
        public SoundEffectAsset SecretSound;

        public GameManager()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Ecs = new Ecs(this);
            Window.AllowUserResizing = true;
        }

        // Intialise will automatically call LoadContent -> no need to override it
        //protected override void Initialize() => base.Initialize();

        protected override void LoadContent()
        {
            AssetManager.Initialise(GraphicsDevice);
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Ecs.Initialize();

            Birb = Asset.Birb.Load();
            Birb2 = Asset.Birb2.Load();
            Birb3 = Asset.Birb3.Load();
            SecretSound = Asset.Secret.Load();

            Entity uiParent = Ecs.CreateEntity("parent");
            new UITransform(uiParent, new(0, 0), 0.1f, null);
            new Gui(uiParent, 1000, 1000);

            Entity frame = Ecs.CreateEntity("frame");
            new UITransform(frame, new(0, 0), 0.1f, uiParent.GetComponent<UITransform>());
            new UIFrame(frame, 1000, 500, 1000, 1000);
            

            Entity birb = Ecs.CreateEntity("birb");
            new UITransform(birb, new(250, 0), 0.5f, frame.GetComponent<UITransform>());
            new UIFrame(birb, 500, 1000);
            //new UI.UITexture(birb, Birb);
            new UIMouseBind(birb, [(UIMouseEvent.LeftMouseClick, new TestInputEvent(null))]);

            Entity birb2 = Ecs.CreateEntity("birb2");
            new UITransform(birb2, new(0, 0), 0.7f, frame.GetComponent<UITransform>());
            new UIFrame(birb2, 500, 1000);
            //new UI.UITexture(birb2, Birb2);
            new UIMouseBlock(birb2);

            Entity birb3 = Ecs.CreateEntity("birb3");
            new Transform(birb3, new(0, 0), 0);
            new Frame(birb3, 1, 1);
            new World2D.Texture(birb3, Birb3);

            Entity camera = Ecs.CreateEntity("camera");
            new Transform(camera, new(0, 0), 0);
            new Camera(camera, 4, 4);
            new ScreenCamera(camera, 200);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Ecs.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            //this.SpriteBatch.Begin();
            //this.SpriteBatch.Draw(Birb.Texture2D, new Rectangle(0, 0, 800, 480), Color.White);
            //this.SpriteBatch.End();

            Ecs.DrawUpdate(gameTime);
        }
    }
}
