using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ITTracker
{
    public partial class ITTrackerDataBaseContext : DbContext
    {
        public ITTrackerDataBaseContext()
        {
        }

        public ITTrackerDataBaseContext(DbContextOptions<ITTrackerDataBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessRole> AccessRoles { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<Request> Requests { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#error где SERVER_NAME - изменить на название Вашего сервера. Затем, удалить данную строку с ошибкой (строка 31)!!!
                optionsBuilder.UseSqlServer("Server=localhost;Database=ITTrackerDataBase;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessRole>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasColumnName("role");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .HasColumnName("fullName");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("firstName");

                entity.Property(e => e.IdRole).HasColumnName("idRole");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("lastName");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_AccessRoles");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClarifationText)
                    .HasMaxLength(3000)
                    .HasColumnName("clarifationText");

                entity.Property(e => e.CustomerAnswer)
                    .HasMaxLength(3000)
                    .HasColumnName("customerAnswer");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.IdCustomer).HasColumnName("idCustomer");

                entity.Property(e => e.IdEmployee).HasColumnName("idEmployee");

                entity.Property(e => e.IdOrderStatus).HasColumnName("idOrderStatus");

                entity.Property(e => e.IdRequest).HasColumnName("idRequest");

                entity.Property(e => e.IdService).HasColumnName("idService");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.HasOne(d => d.IdCustomerNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdCustomer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Customer");

                entity.HasOne(d => d.IdEmployeeNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdEmployee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Employee");

                entity.HasOne(d => d.IdOrderStatusNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdOrderStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_OrderStatus");

                entity.HasOne(d => d.IdRequestNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdRequest)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Requests");

                entity.HasOne(d => d.IdServiceNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdService)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Service");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.ToTable("OrderStatus");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClarifationText)
                    .HasMaxLength(3000)
                    .HasColumnName("clarifationText");

                entity.Property(e => e.CustomerAnswer)
                    .HasMaxLength(3000)
                    .HasColumnName("customerAnswer");

                entity.Property(e => e.Description)
                    .HasMaxLength(3000)
                    .HasColumnName("description");

                entity.Property(e => e.IdCustomer).HasColumnName("idCustomer");

                entity.Property(e => e.IdStatus).HasColumnName("idStatus");

                entity.HasOne(d => d.IdCustomerNavigation)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.IdCustomer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Requests_Customer");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.IdStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Requests_OrderStatus");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
