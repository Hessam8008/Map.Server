namespace Map.Client.Models
{
    using System.ComponentModel.DataAnnotations;

    using Map.Client.Interfaces;

    public class IMEI : IEntity
    {
        [Required]
        public string Value { get; set; }
    }
}