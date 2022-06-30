using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Entity
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public Guid? UserID { get; set; }

        [Required]
        public string UserName { get; set; }

        public string FullName { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        //[JsonConverter(typeof(CustomBase64Converter))]
        public byte[] Img { get; set; }

        public string ImgName { get; set; }

        public bool Status { get; set; }

        public string Role { get; set; }

        public DateTime? RegisterDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }

    //internal class CustomBase64Converter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        return true;
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        return System.Text.Encoding.UTF8.GetString((Convert.FromBase64String((string)reader.Value)));
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        writer.WriteValue(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes((string)value)));
    //    }
    //}
}
