using Application.Common.Interfaces;
using MediatR;

namespace Application.Commands.User;

public class RegisterUser
{
    public record Command(string FullName, string Email, string Password) : IRequest<Guid>;

    public class Handler : IRequestHandler<Command, Guid>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _identityService.CreateUserAsync(request.FullName, request.Email, request.Password);
        }
    }
}