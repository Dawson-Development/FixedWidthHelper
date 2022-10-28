using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace FixedWidthHelper
{
    public class RecordParser
    {
        public RecordParser(Type Class)
        {
            _Context = new ReadingContext(null, Class);
        }

        public RecordParser(TextReader reader, Type Class)
        {
            _Context = new ReadingContext(reader, Class);
        }

        private ReadingContext _Context { get; }
        public virtual ReadingContext Context => _Context;

        public virtual bool Read()
        {
            return ReadLine();
        }

        private bool ReadLine()
        {
            Context.RecordChars = Context.Reader.ReadLine();
            if (Context.RecordChars == null) return false;
            if (Context.FileAttribute.PadFile)
                Context.RecordChars = Context.RecordChars.PadRight(Context.TotalRowLength);

            Context.RowIndex++;
            Context.FieldIndex = 0;
            Context.RecordCharPosition = 0;
            return true;

        }

        public virtual object GetRecord<T>()
        {
            Context.Record = Activator.CreateInstance(typeof(T));
            foreach (var field in Context.FieldAttributes)
                if (field.FieldAttribute.Length > 0 || field.FieldAttribute.ClassObject != null)
                {
                    var fieldVal = ReadField(field);
                    if (fieldVal != null)
                        field.Property.SetValue(Context.Record, fieldVal);
                }

            return Context.Record;
        }

        public virtual T GetRecord<T>(string DataText)
        {
            Context.FieldIndex = 0;
            Context.RecordCharPosition = 0;
            Context.FileAttribute.HasHeaderRecord = false;
            Context.RecordChars = DataText;
            if (Context.FileAttribute.PadFile)
                Context.RecordChars = Context.RecordChars.PadRight(Context.TotalRowLength);

            Context.Record = Activator.CreateInstance(typeof(T));

            foreach (var field in Context.FieldAttributes)
                if (field.FieldAttribute.Length > 0 || field.FieldAttribute.ClassObject != null)
                {
                    var fieldVal = ReadField(field);
                    if (fieldVal != null)
                        field.Property.SetValue(Context.Record, fieldVal);
                }

            return (T)Context.Record;
        }

        private object ReadField(FixedField field)
        {
            var fieldValues = new ArrayList();
            var fieldType = field.Property.PropertyType.GetElementType() ?? field.Property.PropertyType;
            for (var i = 0; i < (field.FieldAttribute.Repeat > 0 ? field.FieldAttribute.Repeat : 1); i++)
                if (field.SubFields != null && field.SubFields.Any())
                {
                    var fieldObject = Activator.CreateInstance(field.FieldAttribute.ClassObject);
                    foreach (var subField in field.SubFields)
                        subField.Property.SetValue(fieldObject, ReadField(subField));
                    fieldValues.Add(fieldObject);
                }
                else
                {
                    Context.FieldValue =
                        Context.RecordChars.Substring(Context.RecordCharPosition, field.FieldAttribute.Length);
                    if (field.FieldAttribute.Trim) Context.FieldValue = Context.FieldValue.Trim();
                    try
                    {
                        fieldValues.Add(Context.FieldValue.Length > 0
                            ? Convert.ChangeType(Context.FieldValue, fieldType)
                            : null);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    Context.RecordCharPosition += field.FieldAttribute.Length;
                }

            Context.FieldIndex++;

            return fieldValues.Count <= 1 ? fieldValues[0] : fieldValues.ToArray(fieldType);
        }
    }
}