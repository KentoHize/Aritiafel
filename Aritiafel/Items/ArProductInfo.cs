using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aritiafel.Items
{
    public class ArProductInfo : IEquatable<ArProductInfo>
    {
        public Type Type { get; set; }
        public List<object> Args { get; set; }
        public Dictionary<string, object> Setting { get; set; }
        public ArProductInfo(Type type)
            : this(type, null, null)
        { }

        public ArProductInfo(Type type, params object[] args)
            : this(type, args.ToList(), null)
        { }

        public ArProductInfo(Type type, List<object> args = null, Dictionary<string, object> settings = null)
        {
            Type = type;
            Args = args ?? new List<object>();
            Setting = settings ?? new Dictionary<string, object>();
        }

        public override int GetHashCode()
            => Type.GetHashCode() ^ Args.GetHashCode() ^ Setting.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (obj is ArProductInfo pi)
                return Equals(pi);
            return false;
        }

        public bool Equals(ArProductInfo other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (Type != other.Type)
                return false;
            if (!Args.SequenceEqual(other.Args))
                return false;
            if (!Setting.SequenceEqual(other.Setting))
                return false;
            return true;
        }

        public static bool operator ==(ArProductInfo p1, ArProductInfo p2)
            => Equals(p1, p2);
        public static bool operator !=(ArProductInfo p1, ArProductInfo p2)
            => !Equals(p1, p2);
    }
}
