using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Contracts.DataContracts;

namespace SearchManagementService.Data
{
    public class SearchManagementServiceContext : DbContext
    {
        public SearchManagementServiceContext (DbContextOptions<SearchManagementServiceContext> options)
            : base(options)
        {
        }

        public DbSet<Contracts.DataContracts.PostModel> PostModel { get; set; } = default!;
    }
}
