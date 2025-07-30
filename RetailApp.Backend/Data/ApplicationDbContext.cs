using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Models;

namespace RetailApp.Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Define DbSets para cada una de tus entidades que quieres mapear a tablas
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Role> Roles { get; set; } = default!;
        public DbSet<UserRole> UserRoles { get; set; } = default!;

        public DbSet<Brand> Brands { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<ProductVariant> ProductVariants { get; set; } = default!;

        public DbSet<Store> Stores { get; set; } = default!;
        public DbSet<Warehouse> Warehouses { get; set; } = default!;
        public DbSet<StoreProduct> StoreProducts { get; set; } = default!;
        public DbSet<WarehouseProduct> WarehouseProducts { get; set; } = default!;

        // --- Nuevos DbSets para las entidades que acabamos de crear ---
        public DbSet<ProductImage> ProductImages { get; set; } = default!; // DbSet para la tabla de Imágenes de Producto
        public DbSet<Collection> Collections { get; set; } = default!;     // DbSet para la tabla de Colecciones
        public DbSet<Inventory> Inventories { get; set; } = default!;     // DbSet para la tabla de Inventario
        public DbSet<Cart> Carts { get; set; } = default!;                 // DbSet para la tabla de Carritos
        public DbSet<CartItem> CartItems { get; set; } = default!;         // DbSet para la tabla de Items de Carrito
        public DbSet<Order> Orders { get; set; } = default!;               // DbSet para la tabla de Pedidos
        public DbSet<OrderItem> OrderItems { get; set; } = default!;       // DbSet para la tabla de Items de Pedido
        // --- Fin de Nuevos DbSets ---


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ... (Mantén todas las configuraciones existentes aquí) ...

            // Configuración para UserRole (clave compuesta)
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // Configuración de la relación para Category (self-referencing para ParentCategoryId)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.ChildCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .IsRequired(false);

            // Configurar que URL_Slug sea único en Products
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.URL_Slug)
                .IsUnique();

            // Configurar clave compuesta para StoreProduct
            modelBuilder.Entity<StoreProduct>()
                .HasKey(sp => new { sp.StoreId, sp.ProductId });

            modelBuilder.Entity<StoreProduct>()
                .HasOne(sp => sp.Store)
                .WithMany(s => s.StoreProducts)
                .HasForeignKey(sp => sp.StoreId);

            modelBuilder.Entity<StoreProduct>()
                .HasOne(sp => sp.Product)
                .WithMany(p => p.StoreProducts)
                .HasForeignKey(sp => sp.ProductId);

            // Configurar clave compuesta para WarehouseProduct
            modelBuilder.Entity<WarehouseProduct>()
                .HasKey(wp => new { wp.WarehouseId, wp.ProductId });

            modelBuilder.Entity<WarehouseProduct>()
                .HasOne(wp => wp.Warehouse)
                .WithMany(w => w.WarehouseProducts)
                .HasForeignKey(wp => wp.WarehouseId);

            modelBuilder.Entity<WarehouseProduct>()
                .HasOne(wp => wp.Product)
                .WithMany(p => p.WarehouseProducts)
                .HasForeignKey(wp => wp.ProductId);

            // --- Configuración para las nuevas entidades (si es necesaria) ---
            // Para ProductImage, CartItem, OrderItem, Inventory, Cart, Order, Collection
            // sus relaciones 1:N estándar se configuran automáticamente por convención,
            // así que no necesitan código adicional aquí a menos que tengas relaciones no convencionales.

            // Por ejemplo, si un Cart tiene un UserId que es nulo (para carritos anónimos)
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany() // No es una colección específica en User, es una relación uno-a-muchos implícita
                .HasForeignKey(c => c.UserId)
                .IsRequired(false); // Permite que UserId sea nulo


            base.OnModelCreating(modelBuilder);
        }
    }
}
