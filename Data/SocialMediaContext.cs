using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Contracts.DataContracts;

namespace SocialMedia.Data
{
    public class SocialMediaContext : DbContext
    {
        public SocialMediaContext (DbContextOptions<SocialMediaContext> options)
            : base(options)
        {
        }

        public DbSet<Contracts.DataContracts.CommentModel> CommentModel { get; set; } = default!;
    }
}
