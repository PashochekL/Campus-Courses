using System.Text.Json.Serialization;

namespace CampusCourses.Data.Entities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Semester
    {
        Autumn, 
        Spring
    }
}
