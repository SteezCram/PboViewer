using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PboViewer.ViewModels
{
    public class ChecksumWindowViewModel
    {
        public string ChecksumResult { get; set; }
        public string Checksum { get; set; }
        public string File { get; set; }
        public string SHA { get; set; }
    }
}
