using Microsoft.Xna.Framework;
using MonGame.ECS;
using MonGame.Input;
using MonGame.Sound;
using MonGame.World2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Movement
{
    public class PlayerMovement : UpdateProcessor
    {
        bool MoveRight = false;
        bool MoveLeft = false;
        bool MoveUp = false;
        bool MoveDown = false;

        public override Dictionary<Type, Func<Event, EventAction>> Events { get; } = [];

        public PlayerMovement()
        {
            Events.Add(typeof(MoveRightEvent), e => { MoveRight = true; return EventAction.Continue; });
            Events.Add(typeof(MoveLeftEvent), e => { MoveLeft = true; return EventAction.Continue; });
            Events.Add(typeof(MoveUpEvent), e => { MoveUp = true; return EventAction.Continue; });
            Events.Add(typeof(MoveDownEvent), e => { MoveDown = true; return EventAction.Continue; });
        }



        public override void Update(GameTime gameTime, Ecs ecs, GameManager game)
        {
            Vector2 movePosition = GetMovePosition();
            foreach(MoveWithPlayer move in ecs.GetComponents<MoveWithPlayer>())
            {
                Vector2 realMove = movePosition * (float)gameTime.ElapsedGameTime.TotalSeconds * move.MoveSpeed;
                Transform transform = move.Entity.GetComponent<Transform>();
                transform.Position += realMove;
            }


            MoveRight = false;
            MoveLeft = false;
            MoveUp = false;
            MoveDown = false;
        }

        private Vector2 GetMovePosition()
        {
            Vector2 result = Vector2.Zero;
            if (MoveRight && !MoveLeft)
            {
                result += new Vector2(1, 0);
            }
            if (MoveLeft && !MoveRight)
            {
                result += new Vector2(-1, 0);
            }
            if (MoveDown && !MoveUp)
            {
                result += new Vector2(0, 1);
            }
            if (MoveUp && !MoveDown)
            {
                result += new Vector2(0, -1);
            }
            if(result != Vector2.Zero)
                result.Normalize();
            return result;
        }
    }
}
