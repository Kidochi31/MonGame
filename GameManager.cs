using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonGame.Assets;
using MonGame.Drawing;
using MonGame.ECS;
using MonGame.Input;
using MonGame.UI;
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

            Entity uiParent = Ecs.CreateEntity("parent");
            new UITransform(uiParent, new(0, 0), 0, null);
            new Gui(uiParent, 1000, 1000);

            Entity frame = Ecs.CreateEntity("frame");
            new UITransform(frame, new(0, 0), 0, uiParent.GetComponent<UITransform>());
            new Frame(frame, 1000, 500, 1000, 1000);
            new UI.Texture(frame, Birb);

            Entity image = Ecs.CreateEntity("image");
            new UITransform(image, new(0, 0), 0.5f, frame.GetComponent<UITransform>());
            new Frame(image, 500, 1000);
            new UI.Texture(image, Birb);
            new UIMouseBind(image, [(UIMouseEvent.MouseOver, new PrintEvent(null))]);
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
