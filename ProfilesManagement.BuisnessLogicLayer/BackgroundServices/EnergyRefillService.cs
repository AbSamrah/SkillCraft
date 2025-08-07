using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProfilesManagement.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProfilesManagement.BuisnessLogicLayer.BackgroundServices
{
    public class EnergyRefillService : BackgroundService
    {
        private readonly ILogger<EnergyRefillService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public EnergyRefillService(ILogger<EnergyRefillService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Energy Refill Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await RefillEnergyForAllUsers();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while refilling energy.");
                }

                
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }

            _logger.LogInformation("Energy Refill Service is stopping.");
        }

        private async Task RefillEnergyForAllUsers()
        {
            _logger.LogInformation("Starting weekly energy refill check...");

            using (var scope = _scopeFactory.CreateScope())
            {
                var profileRepository = scope.ServiceProvider.GetRequiredService<IProfileRepository>();

                var allProfiles = await profileRepository.GetAll();

                foreach (var profile in allProfiles)
                {
                    
                    if (profile.LastEnergyRefill.AddDays(7) <= DateTime.UtcNow)
                    {
                        profile.Energy = 100;
                        profile.LastEnergyRefill = DateTime.UtcNow;

                        await profileRepository.Update(profile);
                        _logger.LogInformation($"Refilled energy for user {profile.Id}.");
                    }
                }
            }
            _logger.LogInformation("Energy refill check completed.");
        }
    }
}