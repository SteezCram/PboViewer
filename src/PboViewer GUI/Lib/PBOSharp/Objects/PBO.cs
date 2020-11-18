using System.Collections.Generic;

namespace PBOSharp.Objects
{
    public class PBO
    {
        ///PBO Prefix 
        public PBOPrefix Prefix { get; internal set; }

        ///List of all files found within the pbo 
        public List<PBOFile> Files { get; internal set; } = new List<PBOFile>();

        ///Whether or not the PBO has been Analysed for its contents
        public bool PBOAnalysed { get; internal set; }

        /// Short form name 
        public string ShortName { get; internal set; }

        ///Long form name
        public string LongName { get; internal set; }

        internal PBOReader Reader { get; set; }
    }
}
