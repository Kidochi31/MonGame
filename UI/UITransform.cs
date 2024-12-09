using Microsoft.Xna.Framework;
using MonGame.ECS;
using MonGame.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MonGame.Drawing
{
    public sealed record class UITransform(Entity Entity, Point Position, float Depth, Component<UITransform>? Parent, List<Component<UITransform>>? Children = null) : ComponentBase(Entity)
    {
        public Point Position { get; set; } = Position;
        public float Depth { get; set; } = Depth;
        public Component<UITransform>? Parent { get; private set;} = Parent;
        public List<Component<UITransform>> Children { get; private set; } = Children?.ToList() ?? [];

        public void AddChild(UITransform child)
        {
            // remove the previous parent
            if (child.Parent is Component<UITransform> oldParent)
                oldParent.GetComponent().RemoveChild(child);

            //Add it to this one
            child.Parent = new(this);
            Children.Add(new(child));
        }

        public void RemoveChild(UITransform child)
        {
            // ensure the child is a child
            if (Children.All(c => !ReferenceEquals(c.GetComponent(), child)))
                throw new NotAChildException(this, child);

            // remove it from the list
            Children.RemoveAll(c => ReferenceEquals(c.GetComponent(), child));

            // set the child's parent to be null
            child.Parent = null;
        }

        protected override void OnCreate(GameManager game)
        {
            var startParent = Parent;
            Parent = null;
            if (startParent is Component<UITransform> parent)
                parent.GetComponent().AddChild(this);
             
            var startChildren = Children;
            Children = new();
            foreach(var child in startChildren)
            {
                AddChild(child.GetComponent());
            }
        }

        protected override void OnDestroy(GameManager game)
        {
            if(Parent is Component<UITransform> parent)
                parent.GetComponent().RemoveChild(this);
            foreach(var child in Children)
            {
                RemoveChild(child.GetComponent());
            }
        }
    }

}
