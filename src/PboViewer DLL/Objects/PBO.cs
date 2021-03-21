using System.Collections.Generic;

namespace PboViewer_Lib.Objects
{
    public class PBO
    {
        /// <summary>
        /// SHA1 checksum of the PBO
        /// </summary>
        public byte[] Checksum { get; internal set; }

        /// <summary>
        /// List of the PBO file
        /// </summary>
        public List<PBOFile> Files { get; internal set; } = new List<PBOFile>();

        /// <summary>
        /// If the PBO has been analysed or not
        /// </summary>
        public bool IsAnalysed { get; internal set; }

        /// <summary>
        /// File path of the PBO
        /// </summary>
        public string LongName { get; internal set; }

        /// <summary>
        /// PBO prefix
        /// </summary>
        public PBOPrefix Prefix { get; internal set; }

        /// <summary>
        /// Reader of the PBO
        /// </summary>
        internal PBOReader Reader { get; set; }

        /// <summary>
        /// File name of the PBO
        /// </summary>
        public string ShortName { get; internal set; }
    }
}
