 //[FixedWidthFile(Arguments)]
    //  Mark class as Fixed Width File.   
    //Arguments: na   
    //Properties:   
    //  PadFile [bool](true): Set whether or not to pad each line the total length of all Field Chars.   
    [FixedWidthFile(PadFile = true)]   
    public class Fw_Test   
    {   
        //[FixedWidthField(Arguments, Properties: [Length, Trim])]   
        //  Mark properties as Fixed Width Fields.   
        //Arguments:   
        //  HeaderName [string](null): If set it will be added to the context.FieldHeaders and the parsed class array.   
        //Properties:    
        //  Length [int](null): The fixed length of the field.   
        //  - If not set this field will be ignored durring import.   
        //  Trim [bool](true): Trim white space before and after field value.   
        //  Repeat [int](null): Repeat this field x times   
   
        [FixedWidthField("Field One", Length = 15)]   
        public string One { get; set; }   

        /// <summary>   
        //You can group repeated sets of fields together using a secondary class.
        //If you define headers in the secondary class it creates "bucketed" groups (A_1|B_1, A_2|B_2)
        //Otherwise define a secondary field and collect the values you need from the array
        /// </summary>
        //[FixedWidthField(typeof(Fw_Test_Object), Repeat = 3)]
        //private Fw_Test_Object[] _SubClass { get; set; }

        //[FixedWidthField("Test Field")]
        //public string SubClass => JsonConvert.SerializeObject(
        //    _SubClass.Select(x => x.ToArray()).ToList()
        //);

        [FixedWidthField(typeof(Fw_Test_Bucket), Repeat = 3)]
        public Fw_Test_Bucket[] _SubClass { get; set; }

        public List<Fw_Test_Bucket> SubClass
        {
            get => _SubClass.ToList();
            set => _SubClass = SubClass.ToArray();
        }

        [FixedWidthField("Field Four", Length = 15)]
        public string Four { get; set; }

        [FixedWidthField("Field Five", Length = 14)]
        public string Five { get; set; }

        [FixedWidthField("Field Six", Length = 9)]
        public string Six { get; set; }

        public string[] ToArray()
        {
            return new List<string>
            {
                JsonConvert.SerializeObject(One),
                Four,
                Five,
                Six
            }.ToArray();
        }
    }

    public class Fw_Test_Bucket
    {
        [FixedWidthField("SubFieldOne_", Length = 15)]
        public string SubFieldOne { get; set; }

        [FixedWidthField("SubFieldTwo_", Length = 12)]
        public string SubFieldTwo { get; set; }
    }

    public class Fw_Test_Object
    {
        [FixedWidthField(Length = 15)] public string SubFieldOne { get; set; }

        [FixedWidthField(Length = 12)] public string SubFieldTwo { get; set; }

        public string[] ToArray()
        {
            return new[]
            {
                SubFieldOne,
                SubFieldTwo
            };
        }
    }
