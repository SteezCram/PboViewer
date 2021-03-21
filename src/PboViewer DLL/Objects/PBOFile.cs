namespace PboViewer_Lib.Objects
{
    public class PBOFile
    {
        /// <summary>
        /// Inner file path in the PBO
        /// </summary>
        public string FileName { get; internal set; }
        /// <summary>
        /// File name
        /// </summary>
        public string FileNameShort { get; internal set; }
        /// <summary>
        /// Packing method in the PBO
        /// </summary>
        public PackingMethod PackingMethod { get; internal set; }
        /// <summary>
        /// Original size of the file: uncompressed final size
        /// </summary>
        public int OriginalSize { get; internal set; }
        /// <summary>
        /// Reserved field
        /// </summary>
        public int Reserved { get; internal set; }
        /// <summary>
        /// Timestamp of the file
        /// </summary>
        public int Timestamp{ get; internal set; }
        /// <summary>
        /// Data size in the PBO: can be compressed
        /// </summary>
        public int DataSize { get; internal set; }
        /// <summary>
        /// Offset in the PBO
        /// </summary>
        public long Offset{ get; internal set; }
        /// <summary>
        /// Reader of the PBO
        /// </summary>
        public PBOReader Reader { get; }

        /// <summary>
        /// Constructor used in the analysis
        /// </summary>
        /// 
        /// <param name="fileName"></param>
        /// <param name="fileNameShort"></param>
        /// <param name="packingMethod"></param>
        /// <param name="originalSize"></param>
        /// <param name="reserved"></param>
        /// <param name="timestamp"></param>
        /// <param name="datasize"></param>
        /// <param name="offest"></param>
        /// <param name="reader"></param>
        public PBOFile(string fileName, string fileNameShort, PackingMethod packingMethod, int originalSize, int reserved, int timestamp, int datasize, long offest, PBOReader reader)
        {
            FileName = fileName;
            FileNameShort = fileNameShort;
            PackingMethod = packingMethod;
            OriginalSize = originalSize;
            Reserved = reserved;
            Timestamp = timestamp;
            DataSize = datasize;
            Offset = offest;
            Reader = reader;
        }

        /// <summary>
        /// Constructor in the packing
        /// </summary>
        /// 
        /// <param name="fileName"></param>
        /// <param name="packingMethod"></param>
        /// <param name="originalSize"></param>
        /// <param name="reserved"></param>
        /// <param name="timeSinceSaved"></param>
        /// <param name="datasize"></param>
        /// <param name="offset"></param>
        public PBOFile(string fileName, PackingMethod packingMethod, int originalSize, int reserved, int timeSinceSaved, int datasize, long offset)
        {
            FileName = fileName;
            PackingMethod = packingMethod;
            OriginalSize = originalSize;
            Reserved = reserved;
            Timestamp = timeSinceSaved;
            DataSize = datasize;
            Offset = offset;
        }   
    }
}
