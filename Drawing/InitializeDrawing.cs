using Microsoft.Xna.Framework;
using MonGame.Assets;
using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Drawing
{
    [DrawProcessor]
    [FirstProcessor]
    public class InitializeDrawing : Processor
    {
        public Texture2DAsset Birb;

        

        public override void Initialize(Ecs ecs, GameManager game)
        {
            Birb = Asset.Birb.Load();
        }

        public override void OnUpdate(GameTime gameTime, Ecs ecs, GameManager gameManager)
        {
            gameManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            gameManager.SpriteBatch.Begin();

            gameManager.SpriteBatch.Draw(Birb.Texture2D, new Rectangle(0, 0, 800, 480), Color.White);
            
        }
    }

    
}
