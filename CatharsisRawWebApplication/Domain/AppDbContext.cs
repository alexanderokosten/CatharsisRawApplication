using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CatharsisRawWebApplication.Domain.Entities;

namespace CatharsisRawWebApplication.Domain
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TextField> TextFields { get; set; }
        public DbSet<ServiceItem> ServiceItems { get; set; }
        public DbSet<UserOrder> UserOrders { get; set; }
        public DbSet<StatusOrders> statusOrders { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<ServiceItem>()
          .HasMany(e => e.UserOrder)
          .WithOne(e => e.service)
          .HasForeignKey(e => e.serviceId);

            modelBuilder.Entity<StatusOrders>()
            .HasMany(e => e.UserOrders)
            .WithOne(e => e.status)
            .HasForeignKey(e => e.statusId);

            //Добавление роли пользователя

            modelBuilder.Entity<StatusOrders>().HasData(new StatusOrders
            {
                StatusId = 1,
                StatusName="Ожидает рассмотрения"
            });
            modelBuilder.Entity<StatusOrders>().HasData(new StatusOrders
            {
                StatusId = 2,
                StatusName = "Отклонено"
            });

            modelBuilder.Entity<StatusOrders>().HasData(new StatusOrders
            {
                StatusId = 3,
                StatusName = "Принято(Ожидайте звонка)"
            });
            modelBuilder.Entity<StatusOrders>().HasData(new StatusOrders
            {
                StatusId = 4,
                StatusName = "Фотографии в обработке"
            });
            modelBuilder.Entity<StatusOrders>().HasData(new StatusOrders
            {
                StatusId = 5,
                StatusName = "Завершено"
            });

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "44546e06-8719-4ad8-b88a-5557e12qrtyu",
                Name = "user",
                NormalizedName = "USER",

            });

            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = "3b62472e-4f66-49fa-a20f-e8827qw3245",
                UserName = "user",
                NormalizedUserName = "USER",
                Email = "user@email.com",
                NormalizedEmail = "USER@EMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "+79054058055",
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "userpassword"),
                SecurityStamp = string.Empty
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "44546e06-8719-4ad8-b88a-5557e12qrtyu",
                UserId = "3b62472e-4f66-49fa-a20f-e8827qw3245"
            });




            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "44546e06-8719-4ad8-b88a-f271ae9d6eab",
                Name = "admin",
                NormalizedName = "ADMIN"
            });

            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = "3b62472e-4f66-49fa-a20f-e7685b9565d8",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "my@email.com",
                NormalizedEmail = "MY@EMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber="+79043846733",
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "superpassword"),
                SecurityStamp = string.Empty
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "44546e06-8719-4ad8-b88a-f271ae9d6eab",
                UserId = "3b62472e-4f66-49fa-a20f-e7685b9565d8"
            });

            modelBuilder.Entity<TextField>().HasData(new TextField { 
                Id = new Guid("63dc8fa6-07ae-4391-8916-e057f71239ce"), 
                CodeWord = "PageIndex", 
                Title = "Главная"
            });
            modelBuilder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid("70bf165a-700a-4156-91c0-e83fce0a277f"), 
                CodeWord = "PageServices", 
                Title = "Наши услуги"
            });
            modelBuilder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid("4aa76a4c-c59d-409a-84c1-06e6487a137a"), 
                CodeWord = "PageContacts", 
                Title = "Контакты"
            });




        }
    }
}
