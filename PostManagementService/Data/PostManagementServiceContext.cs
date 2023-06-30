using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Contracts.DataContracts;

namespace PostManagementService.Data
{
    public class PostManagementServiceContext : DbContext
    {
        public PostManagementServiceContext (DbContextOptions<PostManagementServiceContext> options)
            : base(options)
        {
        }

        public DbSet<Contracts.DataContracts.PostModel> PostModel { get; set; } = default!;
    }
}
