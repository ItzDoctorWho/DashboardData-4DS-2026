using System.ComponentModel.DataAnnotations;

namespace DashboardData.Models;

public class Tag
{
    [Key]
    public int Id { get; set; }

    [Required] [StringLength(30)]
    public string Label { get; set; } // Ex: "Critique", "Maintenance"

    // Navigation N-to-N : Un tag est sur PLUSIEURS capteurs
    public ICollection<SensorData> Sensors { get; set; } = new List<SensorData>();
}