using System;
using CoinKOL.EFCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoinKOL.EFCore.Context
{
    public class Kol_DbContext:DbContext
    {
        public Kol_DbContext()
        {
        }
        public DbSet<TranslatedContentEntity> TranslatedContent { get; set; }
        public DbSet<TwitterContentEntity> TwitterContent { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //配置mariadb连接字符串
            optionsBuilder.UseSqlServer("Server=192.168.23.227;Database=CoinKOL_DEV; User=sa;Password=800TS$Dev2018;");
        }
    }
}
