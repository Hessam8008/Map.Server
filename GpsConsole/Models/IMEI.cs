using System.ComponentModel.DataAnnotations;
using GpsConsole.Interfaces;

namespace GpsConsole.Models
{
    public class IMEI : IEntity
    {
        [Required]
        public string Value { get; set; }
    }
}