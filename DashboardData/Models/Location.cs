using System.ComponentModel.DataAnnotations;

namespace DashboardData.Models;

public class Location
{
    [Key]
    public int Id { get; set; }

    [Required] [StringLength(100)]
    public string Name { get; set; }  // Ex: "Salle Serveur"

    public string? Building { get; set; } // Ex: "Bâtiment A"

    // Navigation : Un emplacement contient PLUSIEURS capteurs
    public ICollection<SensorData> Sensors { get; set; } = new List<SensorData>();
}