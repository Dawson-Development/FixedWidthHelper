using System;

namespace FixedWidthHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FixedWidthFieldAttribute : Attribute
    {
        /// <summary>
        ///     FixedWidthField Attribute
        /// </summary>
        public FixedWidthFieldAttribute()
        {
        }

        /// <summary>
        ///     FixedWidthField Attribute
        /// </summary>
        /// <param name="FieldName"></param>
        public FixedWidthFieldAttribute(string FieldName)
        {
            _Name = FieldName;
        }

        public FixedWidthFieldAttribute(Type type)
        {
            _ClassObject = type;
        }

        /// <summary>
        ///     Field Name
        /// </summary>
        private string _Name { get; }

        public virtual string Name => _Name;

        private Type _ClassObject { get; }
        public Type ClassObject => _ClassObject;

        /// <summary>
        ///     Field Character Length
        /// </summary>
        private int _Length { get; set; }

        public virtual int Length
        {
            get => _Length;
            set => _Length = value;
        }

        /// <summary>
        ///     Trim Field
        /// </summary>
        private bool _Trim { get; set; } = true;

        public virtual bool Trim
        {
            get => _Trim;
            set => _Trim = value;
        }

        /// <summary>
        ///     Number of times to repeat the field.
        /// </summary>
        private int _Repeat { get; set; }

        public virtual int Repeat
        {
            get => _Repeat;
            set => _Repeat = value;
        }

        /// <summary>
        ///     Field Output Index
        /// </summary>
        private int _Index { get; set; }

        public int Index
        {
            get => _Index;
            set => _Index = value;
        }
    }
}