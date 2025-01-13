using System.Text.Json.Serialization;

namespace CampusCourses.Data.Entities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StudentStatuses
    {
        InQueue, 
        Accepted, 
        Declined
    }
}
