using GloboticketWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class GloboticketDbContext : DbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Order> Orders { get; set; }

    public string DbPath { get; }

    public GloboticketDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "globoticket.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}