using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MonGame.UI
{
    public class UIDepth : IComparable<UIDepth>
    {
        public List<float> Depths; // ordered from top level to bottom level. Depth is 0 at back, 1 at front

        public UIDepth() { Depths = []; }

        public UIDepth(float depth) { Depths = [depth]; }

        public UIDepth(UIDepth depth, float newDepth)
        {
            Depths = [.. depth.Depths, newDepth];
        }

        /*
         * Orders from back to front (lowest depth to highest)
        */
        public int CompareTo(UIDepth? other)
        {
            if (other is null)
                throw new Exception("Cannot compare to null UIDepth");

            for(int i = 0; i < Math.Min(other.Depths.Count, this.Depths.Count); i++)
            {
                // if the other is higher than this
                if (other.Depths[i] > this.Depths[i])
                {
                    // this is behind it (it comes first)
                    return -1;
                }
                else if (other.Depths[i] < this.Depths[i])
                {
                    // this is after it (it comes last)
                    return 1;
                }
            }
            // we've reached the end of one (or both of them)
            // if they both ended here, return 0
            if (other.Depths.Count == this.Depths.Count)
                return 0;

            // otherwise, the one with the lower count is at the back
            // if the other has higher count, it is higher, so this comes first
            if (other.Depths.Count > this.Depths.Count)
            {
                return -1;
            }
            // otherwise it comes last
            return 1;
        }
    }
}
