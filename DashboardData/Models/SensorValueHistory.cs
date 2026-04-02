using System.ComponentModel.DataAnnotations;

namespace DashboardData.Models;

public class SensorValueHistory
{
    [Key]
    public int Id { get; set; }

    public double Value { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.Now;

    public int SensorDataId { get; set; }

    public SensorData SensorData { get; set; }
}