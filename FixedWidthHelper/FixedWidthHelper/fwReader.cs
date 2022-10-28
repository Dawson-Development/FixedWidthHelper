using System.Collections.Generic;
using System.IO;

namespace FixedWidthHelper
{
    public class FwReader<T> : IReader<T>
    {
        public FwReader(TextReader Reader)
        {
            Parser = new RecordParser(Reader, typeof(T));
            Context = Parser.Context;
        }

        public ReadingContext Context { get; set; }
        private RecordParser Parser { get; }

        public bool Read()
        {
            return Parser.Read();
        }

        public T GetRecord()
        {
            var record = (T)Parser.GetRecord<T>();
            return record;
        }

        public IEnumerable<T> GetRecords()
        {
            var records = new List<T>();
            while (Read()) records.Add(GetRecord());
            return records;
        }

        public void Dispose()
        {
        }

        public T GetRecord(string DataText)
        {
            var record = Parser.GetRecord<T>(DataText);
            return record;
        }
    }
}