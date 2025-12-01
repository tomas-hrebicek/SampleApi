using Sample.Application.DTOs;
using Sample.Application.Interfaces;
using Sample.Domain.Domain;
using Sample.Domain.Interfaces;

namespace Sample.Application.Services
{
    internal class ProductsService : IProductsService
    {
        public ProductsService(IProductsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        private readonly IProductsRepository _repository;

        public async Task<Result<ProductDTO>> GetAsync(int id)
        {
            var product = await _repository.GetAsync(id);

            if (product is null)
            {
                return Result.Failure<ProductDTO>(new NotFoundError($"product id = {id}"));
            }
            else
            {
                return Result.Success(ProductDTO.Map(product));
            }
        }

        public async Task<Result<IEnumerable<ProductDTO>>> ListAsync()
        {
            var products = await _repository.ListAsync();
            return Result.Success(products.Select(x => ProductDTO.Map(x)));
        }

        public async Task<Result<PagedList<ProductDTO>>> ListAsync(PaginationSettingsDTO paginationSettings)
        {
            var settings = PaginationSettingsDTO.Map(paginationSettings);
            var products = await _repository.ListAsync(settings);
            return Result.Success(new PagedList<ProductDTO>(products.Items.Select(x => ProductDTO.Map(x)).ToList(), products.TotalCount, products.PageNumber, products.PageSize));
        }

        public async Task<Result<ProductDTO>> UpdateDescriptionAsync(int productId, ProductDescriptionDTO description)
        {
            var product = await _repository.GetAsync(productId);

            if (product is null)
            {
                return Result.Failure<ProductDTO>(new NotFoundError($"product id = {productId}"));
            }
            else
            {
                ProductDescriptionDTO.Map(description, product);
                _repository.UpdateAsync(product);
                product = await _repository.GetAsync(productId);
                return Result.Success(ProductDTO.Map(product));
            }
        }
    }
}