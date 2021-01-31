using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aritiafel.Characters
{
    /// <summary>
    /// 文件管理員
    /// </summary>
    public class Archivist //Recorder
    {
        public string Name { get; set; }
        public Stream Stream { get => _Stream; set { if (value != null) { _Stream = value; sw = new StreamWriter(_Stream); } else if(sw != null) sw.Dispose(); } }
        private Stream _Stream;
        public IReadOnlyList<string> Records { get => records.AsReadOnly(); }
        private List<string> records  = new List<string>();
        private StreamWriter sw;

        public Archivist()
            : this(null, null)
        { }

        public Archivist(string name)
           : this(name, null)
        { }

        public Archivist(Stream stream)
            : this("", stream)
        { }

        public Archivist(string name, Stream stream)
        {
            Name = name;
            Stream = stream;
        }

        public void WriteRecord(string record)
        {
            if (_Stream != null)
            {
                sw.WriteLine(record);
                sw.Flush();
            }
            records.Add(record);
        }

        public void WriteRecords(List<string> records)
        {
            foreach (string record in records)
                WriteRecord(record);
        }

        public void ClearRecords()
        {
            records.Clear();
        }
    }
}
