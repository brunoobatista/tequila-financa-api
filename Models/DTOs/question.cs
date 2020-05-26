

//namespace Tequila.Models.DTOs
//{
//    public class ApplicationContext : DbContext
//    {
//        public ApplicationContext(DbContextOptions options) : base(options)
//        {

//        }

//        public DbSet<Address> Address { get; set; }
//        public DbSet<User> User { get; set; }
//    }

//    [Table("address")]
//    public class Address
//    {
//        [Column("id")]
//        public long AddressId { get; set; }

//        [Column("user_id")]
//        [ForeignKey("User")]
//        public long UserId { get; set; }
//        public User User { get; set; }
//    }

//    [Table("user")]
//    public class User
//    {
//        [Key, Column("id")]
//        public long UserId { get; set; }

//        public Address Address { get; set; }
//    }
//} 

//User user = context.User.Include("Address").FirstOrDefault(u => u.Id == Id);
//User user = context.User.Include("Address").Where(u => u.Id == Id).FirstOrDefault();

//protected override void OnModelCreating(ModelBuilder builder)
//{
//    builder.Entity<User>()
//            .HasOne(e => e.Address)
//            .WithOne()
//            .HasForeignKey("Address");
//}