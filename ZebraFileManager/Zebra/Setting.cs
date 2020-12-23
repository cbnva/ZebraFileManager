using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ZebraFileManager.Zebra
{
    public class Setting
    {
        [JsonIgnore]
        public string Name { get; set; }

        public string Value { get; set; }

        [JsonIgnore]
        public object DisplayValue
        {
            get
            {
                switch (Type)
                {
                    case SettingType.Bool:
                        var val = Value?.Trim()?.ToLower();
                        return val == "on";
                    default:
                        return Value;
                }
            }
            set { 
            
            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public SettingType Type { get; set; }

        public string Range { get; set; }

        public bool Clone { get; set; }

        public bool Archive { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SettingAccess Access { get; set; }

        public string Default { get; set; }
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
    }

    public enum SettingAccess
    {
        R,
        RW,
        W
    }
}
