using Newtonsoft.Json;
using System;

namespace Infrastructure.ServiceResult
{
    /// <summary>
    /// Default values
    /// Success = false
    /// Message = "Operation failed"
    /// </summary>
    [JsonObject]
    [Serializable]
    public class ResultSet
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public ResultSet()
        {
            Success = false;
            Message = "Operation Failed";
        }



        public ResultSet(bool defaultValue)
        {
            Success = defaultValue;
            Message = defaultValue ? "Operation success" : "Operation failed";
        }
    }

    /// <summary>
    /// Varsayılan değerleri 
    /// Success = false
    /// Message = "İşleminiz gerçekleştirilemedi"
    /// </summary>
    /// 
    [JsonObject]
    [Serializable]
    public class ResultSet<T> : ResultSet
    {
        public T Object { get; set; }
    }
}
