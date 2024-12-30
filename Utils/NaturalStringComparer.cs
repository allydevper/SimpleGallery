using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleGallery.Utils
{
    internal class NaturalStringComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == null || y == null) return 0;

            var xParts = Regex.Split(x, @"(\d+)");
            var yParts = Regex.Split(y, @"(\d+)");

            for (int i = 0; i < Math.Min(xParts.Length, yParts.Length); i++)
            {
                if (int.TryParse(xParts[i], out int xPartNum) && int.TryParse(yParts[i], out int yPartNum))
                {
                    int result = xPartNum.CompareTo(yPartNum);
                    if (result != 0)
                    {
                        return result;
                    }
                }
                else
                {
                    int result = string.Compare(xParts[i], yParts[i], StringComparison.OrdinalIgnoreCase);
                    if (result != 0)
                    {
                        return result;
                    }
                }
            }

            return xParts.Length.CompareTo(yParts.Length);
        }
    }
}