﻿using RoadmapMangement.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.DataAccessLayer.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IRoadmapDbContext _context;

        public UnitOfWork(IRoadmapDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Commit()
        {
            var changeAmount = await _context.SaveChanges();

            return changeAmount > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
         }
    }
}
