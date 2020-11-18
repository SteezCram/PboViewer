using System;
using PBOSharp.Enums;

namespace PBOSharp.Objects
{
    public class PBOFile
    {
        public string FileName { get; internal set; }
        public string FileNameShort { get; internal set; }
        public PackingMethod PackingMethod { get; internal set; }
        public int OriginalSize { get; internal set; }
        public int Reserved { get; internal set; }
        public int Timestamp{ get; internal set; }
        public int DataSize { get; internal set; }
        public long Offset{ get; internal set; }

        public PBOReader Reader { get; }

        //Constructor used is PBO Analysis 
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

        //Constructor used in PBO Packing 
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
