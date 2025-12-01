using Microsoft.AspNetCore.Mvc;
using Sample.Application.DTOs;
using Sample.Domain.Domain;

namespace Sample.Test.Controllers
{
    public class ProductTest
    {
        [Fact]
        public async void ListReturnsData()
        {
            // Arrange
            var repository = new Mock<Application.Interfaces.IProductsService>();
            repository.Setup(x => x.ListAsync()).ReturnsAsync(new List<ProductDTO>()
            {
                new ProductDTO
                {
                    Id = 1,
                    Name = "product name 1",
                    ImgUri = new Uri("https://www.img.test"),
                    Price = 123M
                },
                new ProductDTO
                {
                    Id = 2,
                    Name = "product name 2",
                    ImgUri = new Uri("https://www.img.test"),
                    Price = 321M
                },
                new ProductDTO
                {
                    Id = 3,
                    Name = "product name 3",
                    ImgUri = new Uri("https://www.img.test"),
                    Price = 132M
                }
            });

            var controller = new Api.Controllers.v1.ProductController(repository.Object);

            // Act
            var actionResult = await controller.List();

            // Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.NotNull(okResult);
            var value = Assert.IsAssignableFrom<IEnumerable<ProductDTO>>(actionResult);
            Assert.NotNull(value);
            Assert.Equal(value.Count(), (int)3);
        }

        [Fact]
        public async void ListPaginationReturnsData()
        {
            // Arrange
            List<ProductDTO> items = new List<ProductDTO>()
            {
                new ProductDTO
                {
                    Id = 1,
                    Name = "product name 1",
                    ImgUri = new Uri("https://www.img.test"),
                    Price = 123M
                },
                new ProductDTO
                {
                    Id = 2,
                    Name = "product name 2",
                    ImgUri = new Uri("https://www.img.test"),
                    Price = 321M
                },
                new ProductDTO
                {
                    Id = 3,
                    Name = "product name 3",
                    ImgUri = new Uri("https://www.img.test"),
                    Price = 132M
                }
            };

            var repository = new Mock<Application.Interfaces.IProductsService>();
            repository.Setup(x => x.ListAsync(It.IsAny<PaginationSettingsDTO>())).ReturnsAsync((PaginationSettingsDTO pagination) =>
            {
                PaginationSettings p = PaginationSettingsDTO.Map(pagination);
                var query = items.AsQueryable();
                var totalCount = query.Count();
                var list = query.Skip((p.PageNumber - 1) * p.PageSize).Take(p.PageSize).ToList();
                return new PagedList<ProductDTO>(list, totalCount, p.PageNumber, p.PageSize);
            });

            var controller = new Api.Controllers.v2.ProductController(repository.Object);

            PaginationSettingsDTO pagination = new PaginationSettingsDTO()
            {
                PageNumber = 1,
                PageSize = 2
            };

            // Act
            var actionResult = await controller.ListPagination(pagination);
   
            // Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.NotNull(okResult);
            var value = Assert.IsAssignableFrom<PagedList<ProductDTO>>(actionResult);
            Assert.NotNull(value);
            Assert.NotNull(value.Items);
            Assert.Equal(value.Items.Count, pagination.PageSize);
            Assert.Equal(value.PageCount, (int)2);
            Assert.Equal(value.TotalCount, (int)3);
        }

        [Fact]
        public async void GetReturnsProduct()
        {
            // Arrange
            var repository = new Mock<Application.Interfaces.IProductsService>();
            repository.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => new ProductDTO()
            {
                Id = id,
                Name = "product name",
                ImgUri = new Uri("https://www.img.test"),
                Price = 123M
            });

            var controller = new Api.Controllers.v1.ProductController(repository.Object);

            int expectedProductId = 123;

            // Act
            var actionResult = await controller.Get(expectedProductId);

            // Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.NotNull(okResult);
            var value = Assert.IsAssignableFrom<ProductDTO>(actionResult);
            Assert.NotNull(value);
            Assert.Equal(value.Id, expectedProductId);
        }

        [Fact]
        public async void GetReturnsNotFound()
        {
            // Arrange
            var repository = new Mock<Application.Interfaces.IProductsService>();
            repository.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((ProductDTO)null);

            var controller = new Api.Controllers.v1.ProductController(repository.Object);

            // Act
            var actionResult = await controller.Get(123);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsAssignableFrom<NotFoundResult>(actionResult);
        }

        [Fact]
        public async void UpdateDescriptionReturnsItemWithNewDescription()
        {
            // Arrange
            var repository = new Mock<Application.Interfaces.IProductsService>();
            repository.Setup(x => x.UpdateDescriptionAsync(It.IsAny<int>(), It.IsAny<ProductDescriptionDTO>())).ReturnsAsync((int id, ProductDescriptionDTO description) => new ProductDTO()
            {
                Id = id,
                Name = "testing product",
                ImgUri = new Uri("https://www.img.test"),
                Price = 123M,
                Description = description.Description
            });

            int productId = 123;
            ProductDescriptionDTO description = new ProductDescriptionDTO() { Description = "test description" };

            var controller = new Api.Controllers.v1.ProductController(repository.Object);

            // Act
            var actionResult = await controller.UpdateDescription(productId, description);

            // Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.NotNull(okResult);
            var value = Assert.IsAssignableFrom<ProductDTO>(actionResult);
            Assert.NotNull(value);
            Assert.Equal(value.Id, productId);
            Assert.Equal(value.Description, description.Description);
        }

        [Fact]
        public async void UpdateDescriptionBadProductIdReturnsNotFound()
        {
            // Arrange
            var repository = new Mock<Application.Interfaces.IProductsService>();
            repository.Setup(x => x.UpdateDescriptionAsync(It.IsAny<int>(), It.IsAny<ProductDescriptionDTO>())).ReturnsAsync((int id, ProductDescriptionDTO description) => (ProductDTO)null);

            int productId = 123;
            ProductDescriptionDTO description = new ProductDescriptionDTO() { Description = "test description" };

            var controller = new Api.Controllers.v1.ProductController(repository.Object);

            // Act
            var actionResult = await controller.UpdateDescription(productId, description);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsAssignableFrom<NotFoundResult>(actionResult);
        }
    }
}