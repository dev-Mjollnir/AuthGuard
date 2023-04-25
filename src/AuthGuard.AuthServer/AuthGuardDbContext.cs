using Microsoft.EntityFrameworkCore;
using System;

namespace AuthGuard.AuthServer
{
    public class AuthGuardDbContext : DbContext
    {
        public AuthGuardDbContext(DbContextOptions<AuthGuardDbContext> options) : base(options)
        {

        }
    }
}
