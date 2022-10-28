using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FixedWidthHelper.Attributes;

namespace FixedWidthHelper
{
    public class ReadingContext
    {
        public ReadingContext(TextReader Reader, Type Class)
        {
            _Reader = Reader;

            FileAttribute = Class.GetCustomAttributes(
                typeof(FixedWidthFileAttribute), true
            ).FirstOrDefault() as FixedWidthFileAttribute ?? new FixedWidthFileAttribute();

            GetFixedFields(Class);
            GetFixedFieldHeaders();
        }

        public FixedWidthFileAttribute FileAttribute { get; set; }

        private TextReader _Reader { get; }
        public virtual TextReader Reader => _Reader;

        public virtual int TotalRowLength { get; private set; }

        public virtual string RecordChars { get; set; }

        public virtual int RecordCharLength { get; set; }

        public virtual int RecordCharPosition { get; set; }

        public virtual object Record { get; set; }

        public virtual string FieldValue { get; set; }

        public virtual int RowIndex { get; set; }

        public virtual string[] HeaderRecord { get; set; }

        public virtual FixedField[] FieldAttributes { get; set; }

        public virtual int FieldIndex { get; set; }

        private void GetFixedFields(Type Class)
        {
            var fields = new List<FixedField>();

            //Get the base class first.
            foreach (var p in Class.BaseType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public |
                                                           BindingFlags.Instance))
            {
                var field = new FixedField(p);
                fields.Add(field);
            }

            //Get the class
            foreach (var p in Class.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance |
                                                  BindingFlags.DeclaredOnly))
            {
                var field = new FixedField(p);
                fields.Add(field);
            }

            FieldAttributes = fields.Where(x => x.FieldAttribute != null)
                .OrderBy(x => x.FieldAttribute.Index).ToArray();
        }

        private void GetFixedFieldHeaders()
        {
            if (FileAttribute.HasHeaderRecord && Reader != null) Reader.ReadLine(); //Read the header line

            var headers = new List<string>();
            foreach (var field in FieldAttributes) headers.AddRange(ParseHeaders(field));
            HeaderRecord = headers.ToArray();
        }

        private string[] ParseHeaders(FixedField field, int? fieldIndex = null)
        {
            var fieldHeaders = new List<string>();
            for (var i = 0; i < (field.FieldAttribute.Repeat > 0 ? field.FieldAttribute.Repeat : 1); i++)
            {
                if (field.SubFields != null)
                    foreach (var subField in field.SubFields)
                        fieldHeaders.AddRange(
                            ParseHeaders(subField, i + 1)
                        );

                if (field.FieldAttribute.Length > 0) TotalRowLength += field.FieldAttribute.Length;

                if (field.FieldAttribute.Name != null)
                    fieldHeaders.Add(field.FieldAttribute?.Name +
                                     (field.FieldAttribute.Repeat > 0 ? (i + 1).ToString() : "") + fieldIndex);
            }

            return fieldHeaders.ToArray();
        }
    }

    public class FixedField
    {
        public FixedField(PropertyInfo property)
        {
            _Property = property;
            GetFixedFieldAttributes();
        }

        private PropertyInfo _Property { get; }
        public virtual PropertyInfo Property => _Property;

        private FixedWidthFieldAttribute _FieldAttribute { get; set; }
        public virtual FixedWidthFieldAttribute FieldAttribute => _FieldAttribute;

        private FixedField[] _SubFields { get; set; }
        public virtual FixedField[] SubFields => _SubFields;

        private void GetFixedFieldAttributes()
        {
            if (Attribute.IsDefined(Property, typeof(FixedWidthFieldAttribute)))
            {
                _FieldAttribute =
                    (FixedWidthFieldAttribute)Property.GetCustomAttribute(typeof(FixedWidthFieldAttribute), false);

                if (FieldAttribute.ClassObject != null)
                {
                    var fields = new List<FixedField>();
                    foreach (var p in FieldAttribute.ClassObject.GetProperties())
                        fields.Add(new FixedField(p));
                    _SubFields = fields.Where(x => x.FieldAttribute != null).ToArray();
                }
            }
        }
    }
}