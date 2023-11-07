﻿using System.ComponentModel.DataAnnotations;

namespace Sample.Application.DTOs
{
    /// <summary>
    /// Represents a product.
    /// </summary>
    public record ProductDTO
    {
        /// <summary>
        /// Product unique identificator
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        /// <summary>
        /// Product name
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Uri to product image.
        /// </summary>
        [Required]
        public Uri ImgUri { get; set; }
        /// <summary>
        /// Product price.
        /// </summary>
        [Required]
        public decimal Price { get; set; }
        /// <summary>
        /// Product description.
        /// </summary>
        public string Description { get; set; }
    }
}