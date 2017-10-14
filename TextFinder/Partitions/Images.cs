using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Images - Рисунки
     *
     */
    class Images : Partition
    {
        public int id = 19;

        protected int minLength = 4;

        protected int maxLength = 1024;

        protected bool isSentence = false;

        protected string[] keywords = { "Images", "Image", "Figure", "Fig", "Illustation", "Plot", "Distribution", "Schematic", "Curve", "Contour", "Surface", "Рисунки" };
    }
}
