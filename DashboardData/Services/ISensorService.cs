using DashboardData.Models;
using DashboardData.Data; // Pour accéder à AppDbContext
using Microsoft.EntityFrameworkCore;
namespace DashboardData.Services
{
    public interface ISensorService
    {
        Task<List<SensorData>> GetSensorDataAsync();
        Task AddSensorAsync(SensorData sensorData);
        Task<List<SensorData>> GetCriticalSensorsAsync(double threshold);
        Task<int> GetTotalCountAsync();
        Task<double> GetAverageValueAsync();
        Task<double> GetMaxValueAsync();
    }

    public class SensorService : ISensorService
    {
        private readonly AppDbContext _context;

        // L'Injection de dépendance fait le travail ici :
        // Quand Blazor crée le SensorService, il lui passe automatiquement le DbContext.
        public SensorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SensorData>> GetSensorDataAsync()
        {
            // EF Core traduit Include par un JOIN SQL vers la table Location
            return await _context.Sensors
                .Include(s => s.Location) 
                .ToListAsync();
        }
        public async Task AddSensorAsync(SensorData sensor)
        {
            // 1. On prépare l'ajout en mémoire
            _context.Sensors.Add(sensor);
            
            // 2. On valide la transaction (Génère le INSERT INTO SQL)
            await _context.SaveChangesAsync(); 
        }

        public async Task<List<SensorData>> GetCriticalSensorsAsync(double threshold)
        {
            return await _context.Sensors
                .Include(s => s.Location)
                .Where(s => s.Value > threshold) // Traduit en : WHERE Value > @threshold
                .OrderByDescending(s => s.Value) // Traduit en : ORDER BY Value DESC
                .ToListAsync();                  // Déclenche l'exécution SQL
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Sensors.CountAsync();
        }

        public async Task<double> GetAverageValueAsync()
        {
            // Si la base est vide, AverageAsync plante. On met une sécurité.
            if (!await _context.Sensors.AnyAsync()) return 0;
            
            return await _context.Sensors.AverageAsync(s => s.Value);
        }

        public async Task<double> GetMaxValueAsync()
        {
            return await _context.Sensors.MaxAsync(s => s.Value);
        }
    }
}
