using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PboViewer.Core
{
    /// <summary>
    /// File type to minify
    /// </summary>
    public enum FileType
    {
        Config = 1,
        SQF = 2,
    }

    class Minifier
    {
        #region Public Members

        /// <summary>
        /// File type of the current minifier
        /// </summary>
        public FileType FileType { get; set; }

        /// <summary>
        /// If the minifier remove the comments
        /// </summary>
        public bool KeepComments { get; set; }

        /// <summary>
        /// If the minifier remove the header
        /// </summary>
        public bool KeepHeader { get; set; }

        #endregion

        public Minifier() {  }

        #region Public Methods

        /// <summary>
        /// Uglify a file
        /// </summary>
        /// 
        /// <param name="filePath">File to uglify</param>
        public void Uglify(string filePath)
        {
            string[] minifyLines = Array.Empty<string>();
            string[] fileLines = File.ReadAllLines(filePath);

            switch (FileType)
            {
                case FileType.Config:
                    break;

                case FileType.SQF:
                    break;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Remove the comments from the lines
        /// </summary>
        /// 
        /// <param name="textLines">Text lines to remove the comments</param>
        /// 
        /// <returns>Text lines without the comments</returns>
        private string[] RemoveComments(string[] textLines)
        {
            int textLinesCount = textLines.Length;

            for (int i = 0; i < textLinesCount; i++)
            {
                string currentLine = textLines[i];

                if (currentLine.Contains("//"))
                {
                    string[] splitted = currentLine.Split("//");
                    textLines[i] = (splitted.Length == 0) ? "" : splitted[0].Trim();
                }
            };


            return textLines;
        }

        /// <summary>
        /// Remove the header from the text lines
        /// </summary>
        /// 
        /// <param name="textLines">Text lines to remove the header</param>
        /// 
        /// <returns>Text lines without the header</returns>
        private string[] RemoveHeader(string[] textLines)
        {
            int startIndex = 0;
            int endIndex = 0;
            int textLinesCount = textLines.Length;

            for (int i = 0; i < textLinesCount; i++)
            {
                if (!string.IsNullOrWhiteSpace(textLines[i]) && textLines[i].StartsWith("/*") && startIndex == 0)
                    startIndex = i;
                if (textLines[i].EndsWith("*/") && endIndex == 0)
                    endIndex = i;
            };


            string[] newTextLines = new string[textLinesCount - endIndex];
            Array.Copy(textLines, startIndex, newTextLines, 0, (textLinesCount - endIndex));

            return newTextLines;
        }

        #endregion
    }
}
