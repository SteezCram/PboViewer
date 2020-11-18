using System;
using System.IO;
using PBOSharp.Objects;
using PBOSharp.Enums;
using System.Text;

namespace PBOSharp
{
    public class PBOSharpClient : IDisposable
    {

        public event PBOSharpEventHandler onEvent;
        private FileStream _fileReader;

        /// <summary>
        /// Analyzes the contents of the PBO File and returns it as a PBO object
        /// </summary>
        /// <param name="File"></param>
        public PBO AnalyzePBO(string file)
        {
            try
            {
                PushOnEvent("AnalyzePBO Started", EventType.Debug);

                //Create A PBO object to store all the PBO's attributes
                PBO pbo = new PBO();
                
                pbo.LongName = file;

                pbo.ShortName = Path.GetFileName(file);

                // Open FileStream
                _fileReader = new FileStream(file, FileMode.Open, FileAccess.Read);

                // Create new instance of PBOReader
                PBOReader _reader = new PBOReader(_fileReader, this);

                // Assign Reader
                pbo.Reader = _reader;

                // Read the header 
                _reader.ReadHeader(pbo);

                // Set analysed flag 
                pbo.PBOAnalysed = true;

                PushOnEvent("AnalyzePBO Finished", EventType.Debug);

                //Return object 
                return pbo;
            }
            catch (Exception ex)
            {
                PushOnEvent(ex.Message, EventType.Error);
                return null;
            }            
        }

        /// <summary>
        /// Extract all files from PBO.
        /// Will extract to extractionDir if specified.
        /// If not will output in the current Dir of the PBO
        /// </summary>
        /// <param name="pbo"></param>
        /// <param name="path"></param>
        public void ExtractAll(PBO pbo, string extractionDir = null)
        {
            try
            {
                PushOnEvent($"Starting full extraction of {pbo.LongName}", EventType.Debug);

                //Get outoput dir depending on if extractionDir was passed 
                string outputDir = extractionDir ?? Path.GetDirectoryName(pbo.LongName);

                //Run through each file in the PBO and write to disk
                foreach (PBOFile file in pbo.Files)
                {
                    //Calculate file name 
                    string fileName = Path.Combine(outputDir, file.FileName);

                    //Write to disk
                    WriteToFile(fileName, ExtractFileData(file));
                }

                PushOnEvent($"Successfully extracted {pbo.LongName}", EventType.Debug);
            }
            catch (Exception ex)
            {
                PushOnEvent($"Failed to extract {pbo.LongName}\n{ex.Message}", EventType.Error);
            }
        }

        /// <summary>
        /// Extract all files from PBO.
        /// Will extract to extractionDir if specified.
        /// If not will output in the current Dir of the PBO
        /// </summary>
        /// <param name="pbo"></param>
        /// <param name="path"></param>
        public void ExtractAll(string filePath, string extractionDir = null)
        {
            if (!File.Exists(filePath))
            {
                PushOnEvent($"File {filePath} does not exist", EventType.Error);
                return;
            }
            
            PBO pbo = AnalyzePBO(filePath);
            try
            {
                PushOnEvent($"Starting full extraction of {pbo.LongName}", EventType.Debug);

                //Get outoput dir depending on if extractionDir was passed 
                string outputDir = extractionDir ?? Path.GetDirectoryName(pbo.LongName);

                if (pbo.Prefix != null)
                    WriteToFile($"{outputDir}\\${pbo.Prefix.PrefixName.ToUpper()}$", Encoding.ASCII.GetBytes(pbo.Prefix.PrefixValue));

                //Run through each file in the PBO and write to disk
                foreach (PBOFile file in pbo.Files)
                {
                    //Calculate file name 
                    string fileName = Path.Combine(outputDir, file.FileName);

                    //Write to disk
                    WriteToFile(fileName, ExtractFileData(file));
                }

                PushOnEvent($"Successfully extracted {pbo.LongName}", EventType.Debug);
            }
            catch (Exception ex)
            {
                PushOnEvent($"Failed to extract {pbo.LongName}\n{ex.Message}", EventType.Error);
            }
        }

        /// <summary>
        /// Extract specific file data and returns it as a byte[]
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public byte[] ExtractFileData(PBOFile file)
        {
            file.Reader.BaseStream.Seek(file.Offset, SeekOrigin.Begin);
            return file.Reader.ReadBytes(file.DataSize);
        }

        /// <summary>
        /// Packs specified folder in to PBO format.
        /// Will output to the parent directory unless packDirectory is specified.
        /// File name will be (folderName.pbo) unless pboName is specified.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="packDirectory"></param>
        /// <param name="pboName"></param>
        public void PackPBO(string folder, string packDirectory = null, string pboName = null)
        {
            try
            {
                PushOnEvent("Starting PackPBO", EventType.Debug);

                //Get the correct pack directory depending on if packDirectory was passed 
                packDirectory = packDirectory ?? Directory.GetParent(folder).FullName;

                //Get the corrent pbo name depending on if pboName was passed 
                pboName = pboName ?? new DirectoryInfo(folder).Name;

                //Make sure the pboName has the correct extension
                if (new FileInfo(pboName).Extension != ".pbo")
                    pboName = $"{pboName}.pbo";

                //Get the final pbo file path
                string pboFilePath = Path.Combine(packDirectory, pboName);

                //make sure the pbo does not alread exist.
                if (File.Exists(pboFilePath))
                {
                    string backup = $"{pboFilePath}.bak";
                    if (File.Exists(backup))
                        File.Delete(backup);
                    File.Move(pboFilePath, backup);
                }


                //Create dir if it does not exist 
                if (!Directory.Exists(Path.GetDirectoryName(pboFilePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(pboFilePath));

                //Create the pbo file 
                FileStream fileStream = new FileStream(pboFilePath, FileMode.Create, FileAccess.Write);

                //Write the file content
                PBOWriter writer = new PBOWriter(fileStream, this);
                writer.WritePBO(folder, packDirectory, pboFilePath);

                fileStream.Dispose();

                PushOnEvent("Finished PackPBO", EventType.Debug);
            }
            catch (Exception ex)
            {
                PushOnEvent(ex.Message, EventType.Error);
            }
        }

        /// <summary>
        /// Write file to disk
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        internal void WriteToFile(string fileName, byte[] data)
        {
            try
            {
                //Create dir if needed 
                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));

                //Open FIle stream 
                FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                //Write data 
                fileStream.Write(data, 0, data.Length);
                //Close 
                fileStream.Dispose();

                PushOnEvent($"{Path.GetFileName(fileName)} -> {fileName}", EventType.Info);
            }
            catch (Exception ex)
            {
                PushOnEvent($"Failed to Write {Path.GetFileName(fileName)} to disk\n{ex.Message}", EventType.Error);
            }
        }

        /// <summary>
        /// Push event to event handler 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        internal void PushOnEvent(string message, EventType type)
            => onEvent?.Invoke(new PBOSharpEventArgs(message, type));

        public void Dispose()
        {
            if (_fileReader != null)
            {
                _fileReader.Close();
                _fileReader.Dispose();
            }
        }
    }
}
