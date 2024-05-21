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
        public string RawPaths { get; set; }
        [IgnoreDataMember]
        public List<string> Paths
        {
            get => RawPaths == null ? null : JsonSerializer.Deserialize<List<string>>(RawPaths);
            set => RawPaths = JsonSerializer.Serialize(value);
        }
        public string WatermarkRawPaths { get; set; }
        [IgnoreDataMember]
        public List<string> WatermarkPaths
        {
            get => WatermarkRawPaths == null ? null: JsonSerializer.Deserialize<List<string>>(WatermarkRawPaths);
            set => WatermarkRawPaths = JsonSerializer.Serialize(value);
        }

        public string PartitionKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string RowKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTimeOffset? Timestamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ETag ETag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
