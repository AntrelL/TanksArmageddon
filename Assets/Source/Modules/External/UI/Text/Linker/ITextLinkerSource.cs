using System;

namespace IJunior.UI
{
    public interface ITextLinkerSource<T>
    {
        public event Action<T> ValueChanged;

        public T Value { get; }
    }
}