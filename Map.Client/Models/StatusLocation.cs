using Map.Client.Interfaces;
using Map.Models.AVL;

namespace Map.Client.Models
{
    public class StatusLocation : IEntity
    {
        public Location Location { get; set; }

        public string IMEI { get; set; }    
    }

    public class LastLocation : IEntity
    {
        public Location Location { get; set; }

        public string IMEI { get; set; }
    }
}