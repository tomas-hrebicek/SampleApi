using Sample.Application.DTOs;
using Sample.Application.Interfaces;
using Sample.Domain.Domain;
using Sample.Domain.Entities;
using Sample.Domain.Interfaces;

namespace Sample.Application.Services
{
    internal class UsersService : IUsersService
    {
        public UsersService(IUsersRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        private readonly IUsersRepository _repository;

        public async Task<Result<UserDTO>> GetAsync(int id)
        {
            var user = await _repository.GetAsync(id);
            return user is null 
                ? Result.Failure<UserDTO>(new NotFoundError()) 
                : Result.Success(UserDTO.Map(user));
        }

        public async Task<Result<PagedList<UserDTO>>> ListAsync(PaginationSettingsDTO paginationSettings)
        {
            var settings = PaginationSettingsDTO.Map(paginationSettings);
            var users = await _repository.ListAsync(settings);

            return Result.Success(new PagedList<UserDTO>(users.Items.Select(x => UserDTO.Map(x)).ToList(), users.TotalCount, users.PageNumber, users.PageSize));
        }
    }
}