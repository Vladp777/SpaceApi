namespace SpaceApi.Model
{
    public class LocationOfISS
    {
        public string message { get; set; }
        public string timestamp { get; set; }
        public Position iss_position { get; set; }
    }

    public class Position
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
}
