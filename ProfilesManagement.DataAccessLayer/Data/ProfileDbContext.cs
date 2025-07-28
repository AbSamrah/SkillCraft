using MongoDB.Driver.Core.Clusters;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ProfilesManagement.DataAccessLayer.Interfaces;

namespace ProfilesManagement.DataAccessLayer.Data
{
    public class ProfileDbContext: IProfileDbContext
    {
        private IMongoDatabase Database { get; set; }
        public IClientSessionHandle Session { get; private set; }
        public MongoClient MongoClient { get; private set; }
        private readonly List<Func<Task>> _commands;
        private readonly IConfiguration _configuration;

        public ProfileDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _commands = new List<Func<Task>>();

            var connectionString = _configuration["MongoSettings:Connection"];
            var databaseName = _configuration["MongoSettings:Databases:ProfilesDB"];

            MongoClient = new MongoClient(connectionString);
            Database = MongoClient.GetDatabase(databaseName);

        }


        public async Task<int> SaveChanges()
        {
            if (_commands.Count == 0)
                return 0;

            try
            {
                var commandTasks = _commands.Select(c => c());
                await Task.WhenAll(commandTasks);
                return _commands.Count;
            }
            finally
            {
                _commands.Clear();
            }
        }


        public IMongoCollection<T> GetCollection<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return Database.GetCollection<T>(name);
        }

        public void AddCommand(Func<Task> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            _commands.Add(func);
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
