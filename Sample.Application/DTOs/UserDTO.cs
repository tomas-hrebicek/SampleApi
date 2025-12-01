using Sample.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sample.Application.DTOs
{
    /// <summary>
    /// Represents an user
    /// </summary>
    public record UserDTO
    {
        public static UserDTO Map(User source, UserDTO dest = null)
        {
            dest ??= new UserDTO();

            dest.Id = source.Id;
            dest.Username = source.Username;
            dest.Active = source.Active;

            return dest;
        }

        /// <summary>
        /// User unique identificator
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        /// <summary>
        /// Unique username
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Username { get; set; }

        /// <summary>
        /// True, if user is active
        /// </summary>
        [Required]
        public bool Active { get; set; }
    }
}
