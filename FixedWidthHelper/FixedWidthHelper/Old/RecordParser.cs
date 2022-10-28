using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using FixedWidthHelper.Configuration;

namespace FixedWidthHelper
{
    public class RecordParser<T> where T : class, new()
    {
        RecordContext RecordContext = new RecordContext();
        FileContext Context;

        private T _RecordObject { get; set; }
        public T RecordObject => _RecordObject;

        private ArrayList FieldHeaders { get; set; } = new ArrayList();
        public RecordParser(ref FileContext context) => Context = context;
        
        public void Read() => Context.CurrentRecordLine = Context.TextReader.ReadLine();

        public T GetRecord()
        {
            Read();
            foreach (PropertyInfo p in Context.FieldContext.FwFields) //Loop through the class properties
            {
                if (Attribute.IsDefined(p, typeof(FixedWidthFieldAttribute))) //Get FixedWidthFields
                {
                    FixedWidthFieldAttribute fwField = (FixedWidthFieldAttribute)p.GetCustomAttribute(typeof(FixedWidthFieldAttribute), false);
                    if (fwField.Length > 0) //Make sure it has a length set
                    {
                        ArrayList fieldValues = new ArrayList();
                        for (int i = 0; i < (fwField.Repeat > 0 ? fwField.Repeat : 1); i++)
                        {
                            var fieldChar = Context.CurrentRecordLine.Substring(RecordContext.CharPosition, fwField.Length);
                            if (fwField.Trim) fieldChar = fieldChar.Trim();
                            fieldValues.Add(fieldChar);
                            RecordContext.CharPosition += fwField.Length;
                        }
                        p.SetValue(RecordObject, fieldValues.Count <= 1 ? fieldValues[0] : fieldValues.ToArray(p.PropertyType.GetElementType() ?? p.PropertyType));
                    }
                }
            }

            Context.FieldContext.FwFields.ToList().ForEach(
                x => ParseField(x)   
            );
            Context.FieldHeaders = (string[])FieldHeaders.ToArray(typeof(string));
            return RecordObject;
        }

        private void ParseField(PropertyInfo Property, T RecordObject = null)
        {
            FixedWidthFieldAttribute Attribute = Context.FieldContext.GetAttribute(Property);
            if(Attribute != null)
            {
                if(Attribute.Length > 0)
                {
                    if(Attribute.ClassObject != null)
                    {
                        FieldContext SubClassContext = new FieldContext(Attribute.ClassObject);
                        var SubClass = Property.GetType();
                        SubClassContext.FwFields.ToList().ForEach(
                            x => ParseField(x, )   
                        );
                    }
                }
                FieldHeaders.Add(Attribute?.Name);

               //Property.SetValue(RecordObject, fieldValues.Count <= 1 ? fieldValues[0] : fieldValues.ToArray(p.PropertyType.GetElementType() ?? p.PropertyType));
            }
        }

        private object GetField()
        {
            return null;
        }
    }
}
