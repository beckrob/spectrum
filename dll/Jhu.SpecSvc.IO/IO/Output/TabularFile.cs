using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Jhu.SpecSvc.IO
{
    public class TabularFile : IDisposable
    {
        public enum FileType
        {
            AsciiTabular,
            AsciiCsv,
            VOTable
        }

        public enum LineEnding
        {
            Windows,
            Unix
        }

        private const string AsciiTabularSeparator = "\t ";
        private const string AsciiCsvSeparator = ", ";
        private const string WindowsLineSeparator = "\r\n";
        private const string UnixLineSeparator = "\n";
        private const string CommentIndicator = "#";

        private FileType type;
        private LineEnding ending;
        private Stream outputStream;
        private StreamWriter streamWriter;
        private string columnSeparator;
        private string lineSeparator;

        public TabularFile(FileType type, LineEnding ending, Stream outputStream)
        {
            this.type = type;
            this.ending = ending;
            this.outputStream = outputStream;

            switch (type)
            {
                case FileType.AsciiTabular:
                    columnSeparator = AsciiTabularSeparator;
                    break;
                case FileType.AsciiCsv:
                    columnSeparator = AsciiCsvSeparator;
                    break;
            }

            switch (ending)
            {
                case LineEnding.Windows:
                    lineSeparator = WindowsLineSeparator;
                    break;
                case LineEnding.Unix:
                    lineSeparator = UnixLineSeparator;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void Open()
        {
            switch (type)
            {
                case FileType.AsciiTabular:
                case FileType.AsciiCsv:
                    streamWriter = new StreamWriter(outputStream);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void WriteHeader(IEnumerable<string> headerNames)
        {
            switch (type)
            {
                case FileType.AsciiTabular:
                case FileType.AsciiCsv:
                    {
                        bool first = true;
                        foreach (string n in headerNames)
                        {
                            if (!first)
                            {
                                streamWriter.Write(columnSeparator);
                            }

                            streamWriter.Write(n);

                            first = false;
                        }
                        streamWriter.Write(lineSeparator);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void WriteComment(string comment)
        {
            switch (type)
            {
                case FileType.AsciiTabular:
                case FileType.AsciiCsv:
                    {
                        streamWriter.Write(CommentIndicator);
                        streamWriter.Write(comment);
                        streamWriter.Write(lineSeparator);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void WriteData(IEnumerable<string> data)
        {
            switch (type)
            {
                case FileType.AsciiTabular:
                case FileType.AsciiCsv:
                    {
                        bool first = true;
                        foreach (string d in data)
                        {
                            if (!first)
                            {
                                streamWriter.Write(columnSeparator);
                            }

                            streamWriter.Write(d);

                            first = false;
                        }
                        streamWriter.Write(lineSeparator);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void Close()
        {
            if (streamWriter != null)
            {
                streamWriter.Dispose();
                streamWriter = null;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}
