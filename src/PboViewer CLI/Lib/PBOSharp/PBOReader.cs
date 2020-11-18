using PBOSharp.Enums;
using PBOSharp.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PBOSharp
{
    public class PBOReader : BinaryReader
    {
        private readonly PBOSharpClient _client;

        public PBOReader(Stream stream, PBOSharpClient client)
          : base(stream)
        {
            _client = client;
        }

        /// <summary>
        /// Keeps reading bytes until it encounters 0 then returns as string
        /// </summary>
        /// <returns></returns>
        public override string ReadString()
        {
            //Initialize Byte list
            List<byte> bytes = new List<byte>();
            //Run through bytes from current index until we encounter 0
            for (byte i = ReadByte(); i != 0; i = ReadByte())
                bytes.Add(i);
            return Encoding.ASCII.GetString(bytes.ToArray());
        }

        /// <summary>
        /// Reader through header structs 
        /// </summary>
        /// <returns></returns>
        internal PBOFile ReadDatablock()
        {
            string fileName = ReadString();
            int packingType = ReadInt32();
            int originalSize = ReadInt32();
            int reserved = ReadInt32();
            int timeStamp = ReadInt32();
            int datasize = ReadInt32();
          

            PackingMethod packingMethod = PackingMethod.Uncompressed;
            switch (packingType)
            {
                case 0x00000000:
                    packingMethod = PackingMethod.Uncompressed;
                    break;
                case 0x43707273:
                    packingMethod = PackingMethod.Packed;
                    break;
                case 0x56657273:
                    packingMethod = PackingMethod.Product;
                    break;
            }
           
            return new PBOFile(fileName, Path.GetFileName(fileName), packingMethod, originalSize, reserved, timeStamp, datasize, BaseStream.Position, this);
        }

        internal void ReadHeader(PBO pbo)
        {
            try
            {
                _client.PushOnEvent($"Starting Header Read for {pbo.LongName}", EventType.Debug);

                //Get the signature
                PBOFile sig = ReadDatablock();

                //_client.PushOnEvent($"Signature read Packing Method: {sig.PackingMethod}", EventType.Debug);

                //Look for a prefix 
                if (sig.PackingMethod == PackingMethod.Product)
                {
                    string possiblePrefix;
                    do
                    {
                        possiblePrefix = ReadString();
                        if (possiblePrefix != string.Empty)
                            pbo.Prefix = new PBOPrefix(possiblePrefix, ReadString());
                    } while (possiblePrefix != string.Empty);
                }

                //Read all file header structs  
                PBOFile file;
                do
                {
                    file = ReadDatablock();
                    if (file.FileName != string.Empty)
                    {
                        pbo.Files.Add(file);
                        _client.PushOnEvent($"File found: {file.FileName}", EventType.Info);
                    }
                } while (file.FileName != string.Empty);

                //Update the file offsets for each file 
                foreach (PBOFile pbofile in pbo.Files)
                {
                    pbofile.Offset = BaseStream.Position;
                    BaseStream.Position += pbofile.DataSize;
                }

                _client.PushOnEvent($"Header read successfully from {pbo.LongName}", EventType.Debug);
            }
            catch (Exception ex)
            {
                _client.PushOnEvent($"Failed to read header for {pbo.LongName}\n {ex.Message}", EventType.Error);
            }
        }
    }
}
