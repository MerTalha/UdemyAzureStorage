using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Models
{
    public class UserPicture : ITableEntity
    {
        public List<string> RawPaths { get; set; }
        public List<string> WatermarkRawPaths { get; set; }

        //[JsonIgnore]
        //public List<string> Paths
        //{
        //    get => string.IsNullOrEmpty(RawPaths) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(RawPaths);
        //    set => RawPaths = value == null || value.Count == 0 ? null : JsonSerializer.Serialize(value);
        //}
        //[JsonIgnore]
        //public List<string> WatermarkPaths
        //{
        //    get => string.IsNullOrEmpty(RawPaths) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(WatermarkRawPaths);
        //    set => WatermarkRawPaths = JsonSerializer.Serialize(value);
        //}
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public UserPicture()
        {
            PartitionKey = string.Empty;
            RowKey = string.Empty;
        }
    }
}
