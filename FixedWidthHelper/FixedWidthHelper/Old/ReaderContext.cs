//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace FixedWidthHelper
//{
//    public class ReaderContext<T> where T : class, new()
//    {
//        /// <summary>
//        /// Total Record Character Length
//        /// </summary>
//        private int _TotalChars { get; set; } = 0;
//        public int TotalChars
//        {
//            get { return _TotalChars; }
//        }

//        /// <summary>
//        /// Current Character Position In Record
//        /// </summary>
//        private int _CharPosition { get; set; }
//        public virtual int CharPosition
//        {
//            get { return _CharPosition; }
//            set { _CharPosition = value; }
//        }

//        /// <summary>
//        /// Current Field Index
//        /// </summary>
//        private int _CurrentFieldIndex { get; set; } = 0;
//        public int CurrentFieldIndex
//        {
//            get { return _CurrentFieldIndex; }
//        }

//        /// <summary>
//        /// Current Context Field
//        /// </summary>
//        public FixedWidthField CurrentField
//        {
//            get { return FixedWidthFields[CurrentFieldIndex]; }
//        }

//        /// <summary>
//        /// Current Characters in the record
//        /// </summary>
//        private string _CurrentRecordChars { get; set; }
//        public string CurrentRecordChars
//        {
//            get { return _CurrentRecordChars; }
//            private set
//            {
//                //Pad the reader line if the FixedWidthFileAttribute requests it
//                _CurrentRecordChars = FileAttributes.PadFile ? value.PadRight(TotalChars, ' ') : value;
//            }
//        }

//        /// <summary>
//        /// Current Characters in Field
//        /// </summary>
//        private string _CurrentFieldChars { get; set; }
//        public string CurrentFieldChars
//        {
//            get { return _CurrentFieldChars; }
//        }

//        /// <summary>
//        /// Fixed Width File Attribute
//        /// </summary>
//        private FixedWidthFileAttribute _FileAttributes { get; set; }
//        public FixedWidthFileAttribute FileAttributes
//        {
//            get { return _FileAttributes; }
//        }

//        /// <summary>
//        /// Collection of FixedWidthField
//        /// </summary>
//        private List<FixedWidthField> _FixedWidthFields { get; set; }
//        public List<FixedWidthField> FixedWidthFields
//        {
//            get { return _FixedWidthFields; }
//        }

//        /// <summary>
//        /// Parsed File Headers
//        /// </summary>
//        private string[] _FieldHeaders { get; set; }
//        public string[] FieldHeaders
//        {
//            get { return _FieldHeaders; }
//        }

//        public ReaderContext()
//        {
//            //Throw an exception if this is not a fixed width file
//            if (!Attribute.IsDefined(typeof(T), typeof(FixedWidthFileAttribute)))
//                throw new Exception("Not A Fixed Width File Class");

//            //Gather the file attributes
//            _FileAttributes = (FixedWidthFileAttribute)typeof(T)
//                .GetCustomAttributes(typeof(FixedWidthFileAttribute), true)
//                .FirstOrDefault();

//            //Collect the list of FixedWidthFields
//            GetClassFields();

//            //Throw an exception if no fields are found
//            if (FixedWidthFields.Count <= 0) throw new Exception("No Fixed Width Fields Found");

//            //Collect the FileHeaders
//            GetHeaderFields();
//        }

//        public T ReadRecord(string line)
//        {
//            var Record = new T(); //Create new 
//            CurrentRecordChars = line;
//            _CurrentFieldIndex = 0;
//            _CharPosition = 0;

//            foreach (FixedWidthField field in FixedWidthFields) //Loop through the Fields
//            {
//                //Get FixedWidthFieldAttribute
//                if (field.GetAttribute<FixedWidthFieldAttribute>() is FixedWidthFieldAttribute fwField && fwField.Length > 0)
//                {
//                    field.FieldProperty.SetValue(
//                        Record,
//                        ReadField()
//                    );
//                    _CurrentFieldIndex++;
//                }
//            }
//            return Record;
//        }

//        public object ReadField()
//        {
//            if ((CurrentField.GetAttribute<FixedWidthFieldAttribute>() is FixedWidthFieldAttribute fwField) && fwField.Length > 0)
//            {
//                ArrayList fieldValues = new ArrayList();
                
//                //Get the field or fields depending on the Repeat option
//                for (int i = 0; i < (fwField.Repeat > 0 ? fwField.Repeat : 1); i++)
//                {
//                    _CurrentFieldChars = CurrentRecordChars.Substring( //Set the current field characters
//                        CharPosition,
//                        ((FixedWidthFieldAttribute)CurrentField.Attributes[typeof(FixedWidthFieldAttribute)]).Length
//                    );
//                    fieldValues.Add(fwField.Trim ? CurrentFieldChars.Trim() : CurrentFieldChars); //Add field value to array
//                    CharPosition += ((FixedWidthFieldAttribute)CurrentField.Attributes[typeof(FixedWidthFieldAttribute)]).Length; //Increase CharPosition
//                }

//                //Return the field value back. It checks if the field is repeating and returns the correct Type
//                return fieldValues.Count <= 1 ? fieldValues[0] : fieldValues.ToArray(
//                    CurrentField.FieldProperty.PropertyType.GetElementType() ??
//                    CurrentField.FieldProperty.PropertyType
//                );
//            }
//            return null;
//        }

//        /// <summary>
//        /// Collect all of the ClassFields
//        /// </summary>
//        private void GetClassFields()
//        {
//            _FixedWidthFields = new List<FixedWidthField>();
//            typeof(T).GetProperties().ToList().ForEach(
//                x => _FixedWidthFields.Add(new FixedWidthField(x))  
//            );
//        }

//        /// <summary>
//        /// Collect the FieldHeaders
//        /// </summary>
//        private void GetHeaderFields()
//        {
//            List<string> headers = new List<string>();

//            foreach (FixedWidthField field in FixedWidthFields)
//            {
//                //Get FixedWidthFieldAttribute
//                if (field.GetAttribute<FixedWidthFieldAttribute>() is FixedWidthFieldAttribute fwField)
//                {
//                    if (fwField.Name != null) headers.Add(fwField.Name); //Add field to header array
//                    _TotalChars += fwField.Length; //Increase Total Record Character Length
//                }
//            }
//            _FieldHeaders = headers.ToArray(); //Set FieldHeaders
//        }
//    }

//    public class FixedWidthField
//    {
//        /// <summary>
//        /// PropertyInfo for the field
//        /// </summary>
//        private PropertyInfo _FieldProperty { get; set; }
//        public PropertyInfo FieldProperty
//        {
//            get { return _FieldProperty; }
//        }

//        private Dictionary<Type, Attribute> _Attributes { get; set; } = new Dictionary<Type, Attribute>();
//        public Dictionary<Type, Attribute> Attributes
//        {
//            get { return _Attributes; }
//        }

//        /// <summary>
//        /// Initialize new FixedWidthField Property.
//        /// </summary>
//        /// <param name="propertyInfo"></param>
//        public FixedWidthField(PropertyInfo propertyInfo)
//        {
//            _FieldProperty = propertyInfo;
//            GetAttributes();
//        }

//        private void GetAttributes()
//        {
//            foreach (Attribute att in FieldProperty.GetCustomAttributes()) //Loop through the class properties
//            {
//                Attributes.Add(
//                    att.GetType(),
//                    att
//                );
//            }
//        }

//        /// <summary>
//        /// Return the requested attribute
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <returns></returns>
//        public Attribute GetAttribute<T>()
//        {
//            if (Attributes.ContainsKey(typeof(T)))
//                return Attributes[typeof(T)];
//            return null;
//        }
//    }
//}
