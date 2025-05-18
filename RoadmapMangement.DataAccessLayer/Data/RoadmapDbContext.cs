using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;
using RoadmapMangement.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadmapMangement.DataAccessLayer.Data
{
    public class RoadmapDbContext : IMongoContext
    {
        private IMongoDatabase Database { get; set; }
        public IClientSessionHandle Session { get; private set; }
        public MongoClient MongoClient { get; private set; }
        private readonly List<Func<Task>> _commands;
        private readonly IConfiguration _configuration;
        private readonly bool _supportsTransactions;

        public RoadmapDbContext(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _commands = new List<Func<Task>>();
            ConfigureMongo();

            // Now using the aliased ClusterType
            _supportsTransactions = MongoClient.Cluster.Description.Type == ClusterType.ReplicaSet;
        }   


        public async Task<int> SaveChanges()
        {
            if (_commands.Count == 0)
                return 0;

            try
            {
                if (_supportsTransactions)
                {
                    return await ExecuteWithTransaction();
                }
                else
                {
                    return await ExecuteWithoutTransaction();
                }
            }
            finally
            {
                _commands.Clear();
            }
        }

        private async Task<int> ExecuteWithTransaction()
        {
            using (Session = await MongoClient.StartSessionAsync())
            {
                Session.StartTransaction();
                try
                {
                    var commandTasks = _commands.Select(c => c());
                    await Task.WhenAll(commandTasks);
                    await Session.CommitTransactionAsync();
                    return _commands.Count;
                }
                catch
                {
                    await Session.AbortTransactionAsync();
                    throw;
                }
            }
        }

        private async Task<int> ExecuteWithoutTransaction()
        {
            var commandTasks = _commands.Select(c => c());
            await Task.WhenAll(commandTasks);
            return _commands.Count;
        }

        private void ConfigureMongo()
        {
            if (MongoClient != null) return;

            var connectionString = _configuration["MongoSettings:Connection"]
                ?? throw new InvalidOperationException("MongoDB connection string is missing");

            var databaseName = _configuration["MongoSettings:DatabaseName"]
                ?? throw new InvalidOperationException("MongoDB database name is missing");

            MongoClient = new MongoClient(connectionString);
            Database = MongoClient.GetDatabase(databaseName);
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