using System;

namespace FixedWidthHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FixedWidthFileAttribute : Attribute
    {
        public bool PadFile { get; set; } = true;

        public bool HasHeaderRecord { get; set; } = true;
    }
}