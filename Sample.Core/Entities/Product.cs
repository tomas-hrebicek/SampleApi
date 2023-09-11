﻿using System;

namespace Sample.Core.Entities
{
    /// <summary>
    /// Represents Sample product.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Uri ImgUri { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}