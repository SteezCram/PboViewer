using PboViewer_Lib.Objects;
using System;
using System.IO;
using System.Text;

namespace PboViewer_Lib
{
    /// <summary>
    /// Event type
    /// </summary>
    public enum EventType
    {
        Info = 1,
        Error = 2,
        Debug = 4,
    }

    /// <summary>
    /// Packing method
    /// </summary>
    public enum PackingMethod
    {
        Uncompressed = 0x00000000,
        Packed = 0x43707273,
        Product = 0x56657273,
    }

    /// <summary>
    /// Implementation of reader and writer class to deal with PBO
    /// </summary>
    public class PBOSharpClient : IDisposable
    {
        public event PBOSharpEventHandler OnEvent;

        /// <summary>
        /// PBO file path
        /// </summary>
        public string PboFilePath { get => _pboFilePath; }


        private string _pboFilePath;
        private FileStream _fileReader;


        public PBOSharpClient(string filePath) 
        {
            _pboFilePath = filePath;
        }


        #region Static Methods

        /// <summary>
        /// Extract specific file data and returns it as a byte[]
        /// </summary>
        /// 
        /// <param name="file">PBO file to extract data</param>
        /// 
        /// <returns>Extracted bytes from the PBO file</returns>
        public static byte[] ExtractFileData(PBOFile file)
        {
            file.Reader.BaseStream.Seek(file.Offset, SeekOrigin.Begin);
            return file.Reader.ReadBytes(file.DataSize);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Analyzes the contents of the PBO File and returns it as a PBO object
        /// </summary>
        public PBO AnalyzePBO()
        {
            try
            {
                PushOnEvent("AnalyzePBO Started", EventType.Debug);

                // Open FileStream
                _fileReader = new FileStream(_pboFilePath, FileMode.Open, FileAccess.Read);
                // Create new instance of PBOReader
                PBOReader _reader = new PBOReader(_fileReader, this);

                // Create A PBO object to store all the PBO's attributes
                PBO pbo = new PBO {
                    LongName = _pboFilePath,
                    ShortName = Path.GetFileName(_pboFilePath),
                    Reader = _reader,
                    Checksum = _reader.ReadChecksum(),
                };

                // Read the header 
                _reader.ReadHeader(pbo);
                // Set analysed flag 
                pbo.IsAnalysed = true;

                PushOnEvent("AnalyzePBO Finished", EventType.Debug);

                return pbo;
            }
            catch (Exception ex) {
                PushOnEvent(ex.Message, EventType.Error);
                return null;
            }            
        }

        /// <summary>
        /// Extract all files from PBO.
        /// Will extract to extractionDir if specified.
        /// If not will output in the current Dir of the PBO
        /// </summary>
        /// 
        /// <param name="pbo"></param>
        /// <param name="extractionDir"></param>
        public void ExtractAll(PBO pbo, string extractionDir = null)
        {
            try
            {
                PushOnEvent($"Starting full extraction of {pbo.LongName}", EventType.Debug);

                // Get outoput dir depending on if extractionDir was passed 
                string outputDir = extractionDir ?? Path.GetDirectoryName(pbo.LongName);

                // Run through each file in the PBO and write to disk
                foreach (PBOFile pboFile in pbo.Files) {
                    // Write to disk
                    WriteToFile(Path.Combine(outputDir, pboFile.FileName), ExtractFileData(pboFile));
                }

                PushOnEvent($"Successfully extracted {pbo.LongName}", EventType.Debug);
            }
            catch (Exception ex)  {
                PushOnEvent($"Failed to extract {pbo.LongName}\n{ex.Message}", EventType.Error);
            }
        }

        /// <summary>
        /// Extract all files from PBO.
        /// Will extract to extractionDir if specified.
        /// If not will output in the current Dir of the PBO
        /// </summary>
        /// 
        /// <param name="pbo">File path to the PBO</param>
        /// <param name="path">Destination folder</param>
        public void ExtractAll(string extractionDir = null)
        {
            if (!File.Exists(_pboFilePath)) {
                PushOnEvent($"File {_pboFilePath} does not exist", EventType.Error);
                return;
            }
            
            PBO pbo = AnalyzePBO();

            try
            {
                PushOnEvent($"Starting full extraction of {pbo.LongName}", EventType.Debug);

                // Get outoput dir depending on if extractionDir was passed 
                string outputDir = extractionDir ?? Path.GetDirectoryName(pbo.LongName);

                if (pbo.Prefix != null)
                    WriteToFile($"{outputDir}\\${pbo.Prefix.PrefixName.ToUpper()}$", Encoding.ASCII.GetBytes(pbo.Prefix.PrefixValue));

                // Run through each file in the PBO and write to disk
                foreach (PBOFile file in pbo.Files) {
                    // Write to disk
                    WriteToFile(Path.Combine(outputDir, file.FileName), ExtractFileData(file));
                }

                PushOnEvent($"Successfully extracted {pbo.LongName}", EventType.Debug);
            }
            catch (Exception ex) {
                PushOnEvent($"Failed to extract {pbo.LongName}\n{ex.Message}", EventType.Error);
            }
        }

        /// <summary>
        /// Packs specified folder in to PBO format.
        /// Will output to the parent directory unless packDirectory is specified.
        /// File name will be (folderName.pbo) unless pboName is specified.
        /// </summary>
        /// 
        /// <param name="folder">Folder to pack the PBO</param>
        /// <param name="packDirectory">Destination directory</param>
        /// <param name="pboName">PBO file name</param>
        public void PackPBO(string folder, string packDirectory = null, string pboName = null)
        {
            try
            {
                PushOnEvent("Starting PackPBO", EventType.Debug);

                // Get the correct pack directory depending on if packDirectory was passed 
                packDirectory ??= Directory.GetParent(folder).FullName;
                // Get the corrent pbo name depending on if pboName was passed 
                pboName ??= new DirectoryInfo(folder).Name;

                // Make sure the pboName has the correct extension
                if (new FileInfo(pboName).Extension != ".pbo")
                    pboName = $"{pboName}.pbo";

                // Get the final pbo file path
                string pboFilePath = Path.Combine(packDirectory, pboName);

                // Make sure the pbo does not alread exist.
                if (File.Exists(pboFilePath))
                {
                    string backup = $"{pboFilePath}.bak";
                    if (File.Exists(backup))
                        File.Delete(backup);

                    File.Move(pboFilePath, backup);
                }


                // Create dir if it does not exist 
                if (!Directory.Exists(Path.GetDirectoryName(pboFilePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(pboFilePath));

                // Create the pbo file 
                using FileStream fileStream = new FileStream(pboFilePath, FileMode.Create, FileAccess.ReadWrite);
                // Write the file content
                using PBOWriter writer = new PBOWriter(fileStream, this);
                writer.WritePBO(folder, packDirectory, pboFilePath);
                writer.WriteChceksum();

                PushOnEvent("Finished PackPBO", EventType.Debug);
            }
            catch (Exception ex) {
                PushOnEvent(ex.Message, EventType.Error);
            }
        }

        /// <summary>
        /// Push event to event handler 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public void PushOnEvent(string message, EventType type)
            => OnEvent?.Invoke(new PBOSharpEventArgs(message, type));

        #endregion

        /// <summary>
        /// Dipose the object
        /// </summary>
        public void Dispose()
        {
            if (_fileReader != null) {
                _fileReader.Close();
                _fileReader.Dispose();
            }
        }

        #region Private Methods

        /// <summary>
        /// Write file to disk
        /// </summary>
        /// <param name="filePath">File path destination</param>
        /// <param name="data">Data to write</param>
        private void WriteToFile(string filePath, byte[] data)
        {
            try
            {
                // Create dir if needed
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                // Write the data on the file
                File.WriteAllBytes(filePath, data);

                PushOnEvent($"{Path.GetFileName(filePath)} -> {filePath}", EventType.Info);
            }
            catch (Exception ex) {
                PushOnEvent($"Failed to Write {Path.GetFileName(filePath)} to disk\n{ex.Message}", EventType.Error);
            }
        }

        #endregion
    }
}
