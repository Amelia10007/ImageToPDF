using System;

namespace ImageToPDF
{
    readonly struct Id : IEquatable<Id>, IComparable<Id>
    {
        private static Id nextId = new Id(ulong.MinValue);

        private readonly ulong uniqueNumber;

        private Id(ulong n) => this.uniqueNumber = n;

        public static Id GetNext()
        {
            if (nextId.uniqueNumber == ulong.MaxValue)
            {
                throw new InvalidOperationException($"Cannot generate more {nameof(Id)} instance.");
            }
            var temp = nextId;
            nextId = new Id(nextId.uniqueNumber + 1);
            return temp;
        }

        public override string ToString() => this.uniqueNumber.ToString();

        public bool Equals(Id other) => this.uniqueNumber.Equals(other.uniqueNumber);

        public int CompareTo(Id other) => this.uniqueNumber.CompareTo(other.uniqueNumber);
    }
}
