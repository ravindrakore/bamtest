using System;

namespace Balyasny.Services.Implementation
{
    public class PositionKey : IEquatable<PositionKey>, IFormattable
    {
        public string Portfolio { get; set; }
        public int SecurityMasterId { get; set; }

        private PositionKey(string portfolio, int securityMasterId)
        {
            this.Portfolio = portfolio;
            this.SecurityMasterId = securityMasterId;
        }

        public static PositionKey Create(string portfolio, int securtyMasterId)
        {
            return new PositionKey(portfolio, securtyMasterId);
        }

        #region Equality members

        public bool Equals(PositionKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(this.Portfolio, other.Portfolio) && this.SecurityMasterId == other.SecurityMasterId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PositionKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.Portfolio != null ? this.Portfolio.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.SecurityMasterId;                
                return hashCode;
            }
        }

        #region Implementation of IFormattable

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"{this.Portfolio}-{this.SecurityMasterId}";
        }

        #endregion

        #endregion
    }
}
