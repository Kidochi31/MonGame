using MonGame.ECS;
using MonGame.World2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Movement
{
    [RequiresComponent(typeof(Transform))]
    public sealed record class MoveWithPlayer(Entity Entity, float MoveSpeed) : ComponentBase(Entity)
    {
        public float MoveSpeed { get; set; } = MoveSpeed;
    }
}
