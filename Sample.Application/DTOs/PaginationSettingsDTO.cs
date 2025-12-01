using Sample.Domain.Domain;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sample.Application.DTOs
{
    /// <summary>
    /// Represents paging settings
    /// </summary>
    public record PaginationSettingsDTO
    {
        public static PaginationSettings Map(PaginationSettingsDTO source, PaginationSettings dest = null)
        {
            dest ??= new PaginationSettings();

            dest.PageNumber = source.PageNumber;
            dest.PageSize = source.PageSize;

            return dest;
        }

        /// <summary>
        /// What page to display.
        /// </summary>
        [DefaultValue(1)]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// How much items is on one page.
        /// </summary>
        [DefaultValue(10)]
        [Range(1, 1000)]
        public int PageSize { get; set; } = 10;
    }
}
