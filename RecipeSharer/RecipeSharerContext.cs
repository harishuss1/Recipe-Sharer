using Microsoft.EntityFrameworkCore;

namespace Context;
using Recipes;
using Users;

public class RecipeSharerContext : DbContext
{
      public virtual DbSet<User>? Users { get; set; }
      public virtual DbSet<Recipe>? Recipes { get; set; }
      public virtual DbSet<Ingredient>? Ingredients { get; set; }
      public virtual DbSet<Rating>? Ratings { get; set; }
      public virtual DbSet<Tag>? Tags { get; set; }
      public virtual DbSet<Step>? Steps { get; set; }


      public string HostName { get; set; }
      public string Port { get; set; }
      public string ServiceName { get; set; }
      public string UserName { get; set; }
      public string Password { get; set; }
      public RecipeSharerContext()
      {
            HostName = Environment.GetEnvironmentVariable("ORACLE_DB_HOST");
            Port = Environment.GetEnvironmentVariable("ORACLE_DB_PORT");
            ServiceName = Environment.GetEnvironmentVariable("ORACLE_DB_SERVICE");
            UserName = Environment.GetEnvironmentVariable("ORACLE_DB_USER");
            Password = Environment.GetEnvironmentVariable("ORACLE_DB_PASSWORD");


      }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
            optionsBuilder.UseOracle($"Data Source={HostName}:{Port}/{ServiceName}; " +
            $"User Id={UserName}; Password={Password}");
      }
}