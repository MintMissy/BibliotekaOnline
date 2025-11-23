using Microsoft.EntityFrameworkCore; 
using System;
using TEST.Models;

namespace TEST.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {

        }


        public DbSet<Autor>? autor { get; set; }
        public DbSet<Czytelnicy>? czytelnicy { get; set; }
        public DbSet<Gatunek>? gatunek { get; set; }
        public DbSet<Kopie>? kopie { get; set; }
        public DbSet<Ksiazka>? ksiazka { get; set; }
        public DbSet<Oddzial>? oddzial { get; set; }
        public DbSet<Pracownicy>? pracownicy { get; set; }
        public DbSet<Regal>? regal { get; set; }
        public DbSet<Wydawnictwo>? wydawnictwo { get; set; }
        public DbSet<Wypozyczenia>? wypozyczenia { get; set; }


    }

}
