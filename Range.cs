using System;

namespace ImageToPDF
{
    readonly struct Range<T> where T: IComparable<T>
    {
        public readonly T Start;
        public readonly T End;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Range(T start, T end)
        {
            if (start.CompareTo(end) > 0)
            {
                throw new ArgumentOutOfRangeException($"Argument {nameof(start)} must be equal or less than {nameof(end)}");
            }

            this.Start = start;
            this.End = end;
        }

        public bool Contains(T value) => this.Start.CompareTo(value) <= 0 && this.End.CompareTo(value) >= 0;
    }
}
