using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageToPDF
{
    class ArgumentToken : IEquatable<ArgumentToken>
    {
        public readonly string Token;

        public ArgumentToken(string token) => this.Token = token;

        public bool Equals(ArgumentToken other) => this.Token.Equals(other.Token);

        public override bool Equals(object obj) => obj is ArgumentToken other && this.Equals(other);

        public override int GetHashCode() => this.Token.GetHashCode();

        public override string ToString() => this.Token;

        public static IEnumerable<ArgumentToken> ParseArgument(string[] arguments) => arguments.Select(a => new ArgumentToken(a));
    }
}
