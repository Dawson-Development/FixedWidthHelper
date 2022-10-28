using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixedWidthHelper.Configuration;

namespace FixedWidthHelper
{
    public class FileContext
    {
        public FileContext(Type type, TextReader textReader)
        {
            _FieldContext = new FieldContext(type);
            _TextReader = textReader;
        }

        private FieldContext _FieldContext { get; set; }
        public FieldContext FieldContext => _FieldContext;

        private TextReader _TextReader { get; set; }
        public TextReader TextReader => _TextReader;

        /// <summary>
        /// Total Record Character Length
        /// </summary>
        private int _TotalChars { get; set; } = 0;
        public int TotalChars => _TotalChars;

        /// <summary>
        /// Parsed File Headers
        /// </summary>
        public string[] FieldHeaders { get; set; }
        //public string[] FieldHeaders => _FieldHeaders;

        public string CurrentRecordLine { get; set; }
        //public string CurrentRecordLine => _CurrentRecordLine;
    }
}