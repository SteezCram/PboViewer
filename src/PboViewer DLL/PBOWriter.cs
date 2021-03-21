using PboViewer_Lib.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PboViewer_Lib
{
    /// <summary>
    /// Writer stream for a PBO
    /// </summary>
    public class PBOWriter : BinaryWriter
    {
        private PBOSharpClient _client;

        public PBOWriter(Stream stream, PBOSharpClient client)
            :base(stream)
        {
            _client = client;
        }

        /// <summary>
        /// Write the buffer
        /// </summary>
        /// 
        /// <param name="buffer">Buffer to write</param>
        public override void Write(string buffer)
        {
            base.Write(Encoding.ASCII.GetBytes(buffer));
            base.Write((byte)0);
        }

        /// <summary>
        /// Write the checksum of the file
        /// </summary>
        public void WriteChceksum()
        {
            BaseStream.Seek(0, SeekOrigin.Begin);

            SHA1 sha = SHA1.Create();
            byte[] sha1Bytes = sha.ComputeHash(BaseStream);

            base.Write((byte)0);
            base.Write(sha1Bytes);
        }

        /// <summary>
        /// Takes content of a folder and packs it in to PBO format
        /// </summary>
        /// 
        /// <param name="folder"></param>
        /// <param name="packDirectory"></param>
        /// <param name="pboName"></param>
        public void WritePBO(string folder, string packDirectory, string pboName)
        {
            try
            {
                _client.PushOnEvent("Starting WritePBO", EventType.Debug);
                PBOPrefix prefix = null;
                List<PBOFile> files = new List<PBOFile>();


                // Look for a prefix file 
                string[] possiblePrefixFiles = Directory.GetFiles(folder, "$*$", SearchOption.TopDirectoryOnly);
                if (possiblePrefixFiles.Length > 0)
                    prefix = new PBOPrefix(Path.GetFileName(possiblePrefixFiles[0]).Trim('$').ToLower(), new StreamReader(possiblePrefixFiles[0]).ReadLine());

                // Look for all the files 
                foreach (string file in Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories))
                {
                    FileInfo fileInfo = new FileInfo(file);

                    // Make sure we are not readding the prefix file
                    if (fileInfo.Name[0] != 36 && fileInfo.Name[fileInfo.Name.Length - 1] != 36)
                    {
                        files.Add(new PBOFile(fileInfo.FullName.Substring(folder.Length + 1), PackingMethod.Uncompressed, 
                            (int)fileInfo.Length, 0, (int)(fileInfo.LastWriteTime - new DateTime(1970, 1, 1)).TotalSeconds, 
                            (int)fileInfo.Length, 0L));
                        _client.PushOnEvent($"Found File {fileInfo.FullName}", EventType.Debug);
                    }
                }

                // Write signature 
                Write("");
                Write((int)PackingMethod.Product);

                for (int i = 0; i < 4; i++)
                    Write(0);


                // Write Prefix if it exists 
                if (prefix != null) {
                    Write(prefix.PrefixName);
                    Write(prefix.PrefixValue);
                }

                // Terminate sig and if it exists the prefix
                Write((byte)0);

                // Write all file structs 
                foreach (PBOFile file in files)
                {
                    Write(file.FileName);
                    Write((int)file.PackingMethod);
                    Write(file.OriginalSize);
                    Write(file.Reserved);
                    Write(file.Timestamp);
                    Write(file.DataSize);

                    _client.PushOnEvent($"Wrote Header for: {file.FileName}", EventType.Info);
                }

                // Write final empty struct 
                Write("");
                Write((int)PackingMethod.Uncompressed);

                for (int i = 0; i < 4; i++)
                    Write(0);

                // Write the data for each file 
                foreach (PBOFile file in files)
                {
                    using BinaryReader br = new BinaryReader(new FileStream(Path.Combine(folder, file.FileName), FileMode.Open, FileAccess.Read));
                    Write(br.ReadBytes((int)br.BaseStream.Length));

                    _client.PushOnEvent($"Wrote Data for: {file.FileName}", EventType.Info);
                }

                _client.PushOnEvent("Finished WritePBO", EventType.Debug);
            }
            catch (Exception ex) {
                _client.PushOnEvent($"WritePBO failed\n{ex.Message}", EventType.Error);
            }
        }
    }
}
