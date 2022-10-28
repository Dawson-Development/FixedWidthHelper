//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FixedWidthHelper
//{
//    [AttributeUsage(AttributeTargets.Property)]
//    public class SerializedFieldAttribute : Attribute
//    {
//        private bool _excludeEmptyObjects;
//        private bool _excludeEmptyValues;
//        private bool _useKeyValuePair;

//        public SerializedFieldAttribute() {}

//        /// <summary>
//        /// Exlude entire empty json objects.
//        /// </summary>
//        public virtual bool ExcludeEmptyObjects
//        {
//            get => _excludeEmptyObjects;
//            set => _excludeEmptyObjects = value;
//        }

//        /// <summary>
//        /// Exclude individual empty values.
//        /// </summary>
//        public virtual bool ExcludeEmptyValues
//        {
//            get => _excludeEmptyValues;
//            set => _excludeEmptyValues = value;
//        }

//        /// <summary>
//        /// Use [Key:"Value"] pair. Otherwise it will only contain values ["Value","Value"].
//        /// </summary>
//        public virtual bool UseKeyValuePair
//        {
//            get => _useKeyValuePair;
//            set => _useKeyValuePair = value;
//        }
//    }
//}
