using PboViewer_Lib.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PboViewer_Lib
{
    /// <summary>
    /// Reader stream for a PBO
    /// </summary>
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
            // Initialize Byte list
            List<byte> bytes = new List<byte>();

            // Run through bytes from current index until we encounter 0
            for (byte i = ReadByte(); i != 0; i = ReadByte())
                bytes.Add(i);

            return Encoding.ASCII.GetString(bytes.ToArray());
        }


        /// <summary>
        /// Read the SHA1 checksum of a PBO if it exists
        /// </summary>
        /// 
        /// <param name="pbo">PBO to read</param>
        public byte[] ReadChecksum()
        {
            BaseStream.Seek(BaseStream.Length - 21, SeekOrigin.Begin);

            // Read the end of the file
            byte[] buffer = new byte[21];
            BaseStream.Read(buffer, 0, 21);
            // Reset the position at 0
            BaseStream.Seek(0, SeekOrigin.Begin);

            if (buffer[0] == 0)
            {
                byte[] checksum = new byte[20];
                Array.Copy(buffer, 1, checksum, 0, 20);

                return checksum;
            }

            return new byte[20];
        }

        /// <summary>
        /// Reader through header structs 
        /// </summary>
        /// 
        /// <returns>PBOFile structure</returns>
        public PBOFile ReadDatablock()
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

        /// <summary>
        /// Read the header of the PBO
        /// </summary>
        /// 
        /// <param name="pbo">PBO to read</param>
        public void ReadHeader(PBO pbo)
        {
            try
            {
                _client.PushOnEvent($"Starting Header Read for {pbo.LongName}", EventType.Debug);

                // Get the signature
                PBOFile sig = ReadDatablock();

                _client.PushOnEvent($"Signature read Packing Method: {sig.PackingMethod}", EventType.Debug);

                // Look for a prefix 
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

                // Read all file header structs  
                PBOFile file;

                do
                {
                    file = ReadDatablock();

                    if (file.FileName != string.Empty) {
                        pbo.Files.Add(file);
                        _client.PushOnEvent($"File found: {file.FileName}", EventType.Info);
                    }
                } while (file.FileName != string.Empty);

                // Update the file offsets for each file 
                foreach (PBOFile pbofile in pbo.Files) {
                    pbofile.Offset = BaseStream.Position;
                    BaseStream.Position += pbofile.DataSize;
                }

                _client.PushOnEvent($"Header read successfully from {pbo.LongName}", EventType.Debug);
            }
            catch (Exception ex) {
                _client.PushOnEvent($"Failed to read header for {pbo.LongName}\n {ex.Message}", EventType.Error);
            }
        }
    }
}
