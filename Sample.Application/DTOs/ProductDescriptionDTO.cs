using Sample.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sample.Application.DTOs
{
    /// <summary>
    /// Represents product description data.
    /// </summary>
    public record ProductDescriptionDTO
    {
        public static Product Map(ProductDescriptionDTO source, Product dest = null)
        {
            dest ??= new Product();

            dest.Description = source.Description;

            return dest;
        }

        /// <summary>
        /// product description
        /// </summary>
        [MaxLength(5000)]
        public string Description { get; set; }
    }
}