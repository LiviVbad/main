﻿using AppFramework.EntityFrameworkCore;

namespace AppFramework.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly AppFrameworkDemoDbContext _context;

        public InitialHostDbBuilder(AppFrameworkDemoDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}