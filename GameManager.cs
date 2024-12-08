using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonGame.Assets;
using MonGame.ECS;
using System;

namespace MonGame
{
    public class GameManager : Game
    {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public Ecs Ecs;

        public GameManager()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Ecs = new Ecs(this);
        }

        // Intialise will automatically call LoadContent -> no need to override it
        //protected override void Initialize() => base.Initialize();

        protected override void LoadContent()
        {
            AssetManager.Initialise(GraphicsDevice);
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Ecs.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Ecs.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            Ecs.DrawUpdate(gameTime);
        }
    }
}
