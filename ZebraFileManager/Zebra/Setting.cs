using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ZebraFileManager.Zebra
{
    public class Setting : INotifyPropertyChanged
    {
        string _name;
        [JsonIgnore]
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        string _value;
        public string Value { get => _value; set { SetProperty(ref _value, value); OnPropertyChanged(nameof(DisplayValue)); } }

        static List<string> trueValues = new List<string> { "yes", "on", "true", "ok" };
        static List<string> falseValues = new List<string> { "no", "off", "false", "open" };

        [JsonIgnore]
        public object DisplayValue
        {
            get
            {
                switch (Type)
                {
                    case SettingType.Bool:
                        // Most boolean settings report the valid "range" in the order of `true, false`,
                        // but some switch the order. In order to properly decode this, two lists contain
                        // the possible true and false options. If neither contains the current value,
                        // assume the first option in the `Range` is `true`.
                        if (trueValues.Contains(Value?.ToLower().Trim()))
                            return true;
                        if (falseValues.Contains(Value?.ToLower().Trim()))
                            return false;

                        var options = Range?.Split(',');
                        if (options?.Length == 2)
                            return Value?.Trim() == options[0];
                        return false;
                    default:
                        return Value;
                }
            }
            set
            {
                if (!IsValidValue(value))
                {
                    throw new ArgumentException("Invalid value");
                }
                switch (Type)
                {
                    case SettingType.Bool:
                        var options = Range?.Split(',');
                        if (options?.Length == 2)
                        {
                            var trueValue = options.FirstOrDefault(x => trueValues.Contains(x)) ?? options[0];
                            var falseValue = options.FirstOrDefault(x => falseValues.Contains(x)) ?? options[1];
                            Value = (value as bool?) == true ? trueValue : falseValue;
                        }
                        break;
                    case SettingType.Enum:
                    case SettingType.String:
                        Value = value as string;
                        break;
                    case SettingType.Integer:
                        if (value is string)
                        {
                            Value = long.Parse(value as string).ToString();
                        }
                        else if(value is int || value is long)
                        {
                            Value = (value as long?)?.ToString();
                        }
                        break;
                    case SettingType.IPV4_Address:
                        if (value is string)
                        {
                            Value = value as string;
                        }
                        else if (value is System.Net.IPAddress)
                        {
                            Value = (value as System.Net.IPAddress).ToString();
                        }
                        break;
                    case SettingType.Double:
                        if (value is string)
                        {
                            Value = double.Parse(value as string).ToString();
                        }
                        else
                        {
                            Value = (value as double?)?.ToString();
                        }
                        break;
                    default:
                        break;
                }

            }
        }

        static Regex stringRangeRegex = new Regex(@"^(?<min>\d+)-(?<max>\d+)$", RegexOptions.Compiled);
        static Regex intRangeRegex = new Regex(@"^(?<min>-?\d+)-(?<max>-?\d+)$", RegexOptions.Compiled);
        static Regex doubleRangeRegex = new Regex(@"^(?<min>-?\d+(\.\d+)?)-(?<max>-?\d+(\.\d+)?)$", RegexOptions.Compiled);

        public bool IsValidValue(object value)
        {
            switch (Type)
            {
                case SettingType.Bool:
                    return value is bool;

                case SettingType.Enum:
                    if (value is string)
                    {
                        return string.IsNullOrEmpty((string)value) || Range?.Split(',')?.Contains(value) == true;
                    }
                    return false;

                case SettingType.String:
                    if (value is string && Range is string && stringRangeRegex.IsMatch(Range))
                    {
                        var m = stringRangeRegex.Match(Range);
                        var min = int.Parse(m.Groups["min"].Value);
                        var max = int.Parse(m.Groups["max"].Value);
                        return min <= ((string)value).Length && ((string)value).Length <= max;
                    }
                    return value is string;

                case SettingType.Integer:
                    if ((value is long || value is int || value is string) && Range is string && intRangeRegex.IsMatch(Range))
                    {
                        var m = intRangeRegex.Match(Range);
                        var min = long.Parse(m.Groups["min"].Value);
                        var max = long.Parse(m.Groups["max"].Value);

                        if (value is int || value is long)
                            return min <= (long)value && (long)value <= max;

                        if (value is string && long.TryParse(value as string, out long z))
                            return min <= z && z <= max;
                    }
                    return false;

                case SettingType.IPV4_Address:
                    return (value is string || value is System.Net.IPAddress);

                case SettingType.Double:

                    if ((value is double || value is string) && Range is string && doubleRangeRegex.IsMatch(Range))
                    {
                        var m = doubleRangeRegex.Match(Range);
                        var min = double.Parse(m.Groups["min"].Value);
                        var max = double.Parse(m.Groups["max"].Value);

                        if (value is double)
                            return min <= (double)value && (double)value <= max;

                        if (value is string && double.TryParse(value as string, out double z))
                            return min <= z && z <= max;
                    }

                    return false;
            }

            return false;
        }

        SettingType _type;
        [JsonConverter(typeof(StringEnumConverter))]
        public SettingType Type { get => _type; set => SetProperty(ref _type, value); }

        string _range;
        public string Range { get => _range; set => SetProperty(ref _range, value); }

        bool _clone;
        public bool Clone { get => _clone; set => SetProperty(ref _clone, value); }

        bool _archive;
        public bool Archive { get => _archive; set => SetProperty(ref _archive, value); }

        SettingAccess _access;
        [JsonConverter(typeof(StringEnumConverter))]
        public SettingAccess Access { get => _access; set => SetProperty(ref _access, value); }

        string _default;
        public string Default { get => _default; set => SetProperty(ref _default, value); }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        ///     Checks if a property already matches a desired value.  Sets the property and
        ///     notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">
        ///     Name of the property used to notify listeners.  This
        ///     value is optional and can be provided automatically when invoked from compilers that
        ///     support CallerMemberName.
        /// </param>
        /// <returns>
        ///     True if the value was changed, false if the existing value matched the
        ///     desired value.
        /// </returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        ///     Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">
        ///     Name of the property used to notify listeners.  This
        ///     value is optional and can be provided automatically when invoked from compilers
        ///     that support <see cref="CallerMemberNameAttribute" />.
        /// </param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum SettingType
    {
        [EnumMember(Value = "string")]
        String,
        [EnumMember(Value = "enum")]
        Enum,
        [EnumMember(Value = "bool")]
        Bool,
        [EnumMember(Value = "integer")]
        Integer,
        [EnumMember(Value = "ipv4-address")]
        IPV4_Address,
        [EnumMember(Value = "double")]
        Double,
        [EnumMember(Value = "json")]
        Json,
    }

    public enum SettingAccess
    {
        R,
        RW,
        W
    }
}
