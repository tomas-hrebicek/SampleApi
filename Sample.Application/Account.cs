using Sample.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sample.Application
{
    public class Account
    {
        public static Account Map(User source, Account dest = null)
        {
            dest ??= new Account();

            dest.Id = source.Id;
            dest.Username = source.Username;

            return dest;
        }

        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(256)]
        public string Username { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}