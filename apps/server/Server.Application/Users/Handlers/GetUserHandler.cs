using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.DTOs;
using Server.Application.Users.Queries;
using Server.Core.Results;

namespace Server.API.Controllers
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, Result<UserDTO>>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;

        public GetUserHandler(IAuthRepository authRepository, IUserRepository userRepository)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<UserDTO>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch auth
            var auth = await _authRepository.GetByIdAsync(request.AuthId, cancellationToken);
            if (auth == null)
                return Result<UserDTO>.Failure("User not found", 404);

            // step 2: fetch user
            var user = await _userRepository.GetByAuthIdAsync(request.AuthId, cancellationToken);
            if (user == null)
                return Result<UserDTO>.Failure("User not found", 404);

            // step 3: prepare DTO
            var userDto = new UserDTO
            {
                AuthId = auth.Id,
                UserId = user.Id,
                UserName = auth.UserName,
                Email = auth.Email.ToString(),
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Status = user.Status.ToString(),
                ContactNumber = user.ContactNumber.ToString(),
                IsContactNumberVerified = user.IsContactNumberVerified,
                Gender = user.Gender.ToString(),
                Dob = user.Dob.ToString(),
            };

            // step 4: return
            return Result<UserDTO>.Success(userDto);
        }
    }
}