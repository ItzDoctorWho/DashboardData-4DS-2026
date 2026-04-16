using System.ComponentModel.DataAnnotations;

namespace DashboardData.Models;

public class SensorData
{
    [Key] // Clé Primaire (PK)
    public int Id { get; set; }

    [Required] [StringLength(50, MinimumLength=3, ErrorMessage = "Le nom doit comporter entre 3 et 50 caractères.")]
    public string Name { get; set; }

    public string Type { get; set; } = "Temperature";
    [Required] [Range(-50.0, 150.0, ErrorMessage = "La valeur doit être comprise entre -50 et 150.")]
    public double Value { get; set; }

    public DateTime LastUpdate { get; set; } = DateTime.Now;

    // ===== RELATIONS =====

    // FK vers Location (1-to-N) : chaque capteur est dans UN emplacement
    [Range(1, int.MaxValue, ErrorMessage = "La LocationId doit être un entier positif.")]
    public int LocationId { get; set; }
    public Location Location { get; set; }

    // Navigation N-to-N : un capteur peut avoir PLUSIEURS tags
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();

    // Navigation 1-to-N : un capteur a plusieurs valeurs historisees
    public ICollection<SensorValueHistory> Values { get; set; } = new List<SensorValueHistory>();
}