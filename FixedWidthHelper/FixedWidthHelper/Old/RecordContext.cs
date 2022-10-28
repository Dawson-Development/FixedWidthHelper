using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FixedWidthHelper
{
    public class RecordContext
    {
        /// <summary>
        /// Current Character Position In Record
        /// </summary>
        private int _CharPosition { get; set; }
        public virtual int CharPosition
        {
            get { return _CharPosition; }
            set { _CharPosition = value; }
        }

        /// <summary>
        /// Current Field Index
        /// </summary>
        private int _CurrentFieldIndex { get; set; } = 0;
        public int CurrentFieldIndex
        {
            get { return _CurrentFieldIndex; }
        }

        /// <summary>
        /// Current Characters in the record
        /// </summary>
        //private string _CurrentRecordChars { get; set; }
        //public string CurrentRecordChars
        //{
        //    get { return _CurrentRecordChars; }
        //    private set
        //    {
        //        //Pad the reader line if the FixedWidthFileAttribute requests it
        //        _CurrentRecordChars = FileAttributes.PadFile ? value.PadRight(TotalChars, ' ') : value;
        //    }
        //}

        /// <summary>
        /// Current Characters in Field
        /// </summary>
        private string _CurrentFieldChars { get; set; }
        public string CurrentFieldChars
        {
            get { return _CurrentFieldChars; }
        }

        public PropertyInfo CurrentFieldProperty { get; set; }
        public FixedWidthFieldAttribute CurrentFieldAttribute { get; set; }
    }
}
