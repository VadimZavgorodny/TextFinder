using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace TextFinder {

    class _TypesCheck {
        private Dictionary<string, string> types;

        public _TypesCheck() {
            types = new Dictionary<string, string>();
            InitTypes();
        }

        private bool check(string mime, string extension) {
            try {
                if (types[extension] == mime)
                    return true;
            }
            catch { }

            return false; 
        }

        public bool IsWord(string extension) {
            return check("msword", extension);             
        }

        public bool IsPDF(string extension) {
            return check("pdf", extension);
        }

        private void InitTypes() {
            types.Add(".doc", "msword");
            types.Add(".docx", "msword");
            types.Add(".dot", "msword");
            types.Add("word", "msword");
            types.Add(".pdf", "pdf");
        }
    }
}
