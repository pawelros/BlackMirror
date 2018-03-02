namespace BlackMirror.Models
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class ValueObject<T> : IEquatable<T>
        where T : class
    {
        public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
        {
            return object.Equals(left, right);
        }

        public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
        {
            return !object.Equals(left, right);
        }

        public bool Equals(T other)
        {
            return this.Equals((object)other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.PropertiesAreEqual((ValueObject<T>)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            var properties = this.GetType().GetProperties(BindingFlags.Public);

            foreach (var propertyValue in properties.Select(propertyInfo => propertyInfo.GetValue(this as T)))
            {
                unchecked
                {
                    hashCode = (hashCode * 397) ^ (propertyValue != null ? propertyValue.GetHashCode() : 0);
                }
            }

            return hashCode;
        }

        private bool PropertiesAreEqual(ValueObject<T> other)
        {
            var properties = typeof(T).GetProperties();
            return properties.All(propertyInfo => Equals(propertyInfo.GetValue(this as T), propertyInfo.GetValue(other as T)));
        }
    }
}