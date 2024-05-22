using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Models
{
    public class UserPicture : ITableEntity
    {
        [AllowNull]
        public string RawPaths { get; set; }

        [IgnoreDataMember]
        public List<string> Paths
        {
            get => RawPaths == null ? null : JsonSerializer.Deserialize<List<string>>(RawPaths); 
            
            set => RawPaths = JsonSerializer.Serialize(value);
        }

        [AllowNull]
        public string WatermarkRawPaths { get; set; }

        [IgnoreDataMember]
        public List<string> WatermarkPaths
        {
            get => WatermarkRawPaths == null ? null : JsonSerializer.Deserialize<List<string>>(WatermarkRawPaths);

            set => WatermarkRawPaths = JsonSerializer.Serialize(value);
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
