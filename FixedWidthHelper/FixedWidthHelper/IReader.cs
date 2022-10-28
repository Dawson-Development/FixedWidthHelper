using System;
using System.Collections.Generic;

namespace FixedWidthHelper
{
    public interface IReader<out T> : IDisposable
    {
        bool Read();

        //Task<bool> ReadAsync();

        T GetRecord();

        IEnumerable<T> GetRecords();
    }
}