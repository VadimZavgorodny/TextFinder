using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Aims and Objectives - Цели и задачи работы 
     *
     */
    class AimsObjectives : Partition
    {
        public int id = 15;

        protected int minLength = 64;

        protected int maxLength = 8192;

        protected bool isSentence = true;

        protected string[] keywords = { "Goals", "Aims", "Objectives", "Object of the analysis", "Purpose", "Цели", "Задачи" };
    }
}
