using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM_Console_App.Database;
using Microsoft.EntityFrameworkCore;

namespace ATM_Console_App.Dtos;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       if(!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(MyConnection.connection.ToString());
        }
    }

    public DbSet<User> users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}
