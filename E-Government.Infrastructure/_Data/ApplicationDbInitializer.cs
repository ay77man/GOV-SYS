using E_Government.Core.Domain.Entities;
using E_Government.Core.Domain.RepositoryContracts.Persistence;
using E_Government.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Government.Infrastructure._Data
{
    public class ApplicationDbInitializer : DbInitializer, IDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicationDbInitializer(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task SeedAsnc()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };

            // Seed Customers
            if (!_dbContext.Customers.Any())
            {
                var customerData = await File.ReadAllTextAsync("../E-Government.Infrastructure\\_Data\\Seeds\\Customer.json");
                var customers = JsonSerializer.Deserialize<List<Customer>>(customerData);

                if (customers?.Count > 0)
                {
                    await _dbContext.Customers.AddRangeAsync(customers);
                    await _dbContext.SaveChangesAsync();
                }
            }

            // Seed Meters
            if (!_dbContext.Meters.Any())
            {
                // Clear any tracked entities first
                _dbContext.ChangeTracker.Clear();

                var meterData = await File.ReadAllTextAsync("../E-Government.Infrastructure\\_Data\\Seeds\\meters.json");
                var meters = JsonSerializer.Deserialize<List<Meter>>(meterData, jsonOptions);

                if (meters?.Count > 0)
                {
                    // Ensure no duplicates in the JSON data
                    var distinctMeters = meters
                        .GroupBy(m => m.Id)
                        .Select(g => g.First())
                        .ToList();

                    await _dbContext.Meters.AddRangeAsync(distinctMeters);
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (!_dbContext.Citizens.Any())
            {
                var CitizensData = await File.ReadAllTextAsync("E:\\c#\\gov\\E-Government.Infrastructure\\_Data\\Seeds\\Citizens.json");
                var Citizens = JsonSerializer.Deserialize<List<Citizens>>(CitizensData);

                if (Citizens?.Count > 0)
                {
                    await _dbContext.Citizens.AddRangeAsync(Citizens);
                    await _dbContext.SaveChangesAsync();
                }
            }

            // In your seed method for MeterReadings
            if (!_dbContext.MetersReading.Any())
            {
                // Get all existing meter IDs
                var existingMeterIds = await _dbContext.Meters.Select(m => m.Id).ToListAsync();

                var jsonData = @"
        [
          {
            ""MeterId"": 1,
            ""ReadingDate"": ""2023-01-01"",
            ""Value"": 1250,
            ""IsEstimated"": false
          },
          {
            ""MeterId"": 1,
            ""ReadingDate"": ""2023-02-01"",
            ""Value"": 1450,
            ""IsEstimated"": false
          },
          {
            ""MeterId"": 2,
            ""ReadingDate"": ""2023-01-01"",
            ""Value"": 50,
            ""IsEstimated"": false
          }
        ]";

                var readings = JsonSerializer.Deserialize<List<MeterReading>>(jsonData);

                if (readings?.Count > 0)
                {
                    // Filter to only include readings for meters that exist
                    var validReadings = readings
                        .Where(r => existingMeterIds.Contains(r.MeterId))
                        .ToList();

                    if (validReadings.Any())
                    {
                        await _dbContext.MetersReading.AddRangeAsync(validReadings);
                        await _dbContext.SaveChangesAsync();
                    }



                }

            }
        }
    }
}