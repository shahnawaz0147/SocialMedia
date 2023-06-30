using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Contracts.DataContracts;

namespace CommentsManagementService.Data
{
    public class CommentsManagementServiceContext : DbContext
    {
        public CommentsManagementServiceContext (DbContextOptions<CommentsManagementServiceContext> options)
            : base(options)
        {
        }

        public DbSet<Contracts.DataContracts.CommentModel> CommentModel { get; set; } = default!;
    }
}
