using MonGame.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.UI
{
    public abstract class UIException(string message) : Exception(message) { }

    public class NotAChildException(UITransform parent, UITransform child) : UIException($"{child} is not a child of {parent}!") { }
}
