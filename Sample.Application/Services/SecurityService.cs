using Sample.Application.DTOs;
using Sample.Application.Interfaces;
using Sample.Domain.Entities;
using Sample.Domain.Interfaces;

namespace Sample.Application.Services
{
    /// <summary>
    /// https://code-maze.com/csharp-hashing-salting-passwords-best-practices/
    /// </summary>
    internal class SecurityService : ISecurityService
    {
        private const int HASH_LENGTH = 64;

        public SecurityService(IUsersRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        private readonly IUsersRepository _repository;

        public async Task<Result<UserDTO>> CreateAccount(CreateAccountDTO accountData)
        {
            var existingUser = await _repository.GetAsync(accountData.Username);
            if (existingUser is not null)
            {
                return Result.Failure<UserDTO>(new AlreadyExistsError<string>(accountData.Username));
            }
            
            SecurityProvider passwordProvider = new SecurityProvider();
            var salt = passwordProvider.CreateSalt(HASH_LENGTH);
            var hash = passwordProvider.HashPassword(accountData.Password, salt);

            User newUser = new User()
            {
                Username = accountData.Username,
                Password = hash,
                Salt = salt
            };

            newUser = await _repository.InsertAsync(newUser);
            return Result.Success(UserDTO.Map(newUser));
        }

        public async Task<Result<Account>> Authenticate(string login, string password)
        {
            User user = await _repository.GetAsync(login);

            if (user is null)
            {
                return Result.Failure<Account>(new NotFoundError(login));
            }
            else
            {
                SecurityProvider passwordProvider = new SecurityProvider();
                bool passwordOk = passwordProvider.VerifyPassword(password, user.Password, user.Salt);
                return passwordOk ? Result.Success(Account.Map(user)) : Result.Failure<Account>(new SecurityError("Bad password"));
            }
        }
    }
}