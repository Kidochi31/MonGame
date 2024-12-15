using MonGame.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.World2D
{
    public abstract class World2DException(string message) : Exception(message) { }

    public class NotAChildException(Transform parent, Transform child) : World2DException($"{child} is not a child of {parent}!") { }
}
