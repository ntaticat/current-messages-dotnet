using Application.Common.Interfaces;
using MediatR;

namespace Application.Commands.User;

public class LoginUser
{
    public record Command(string Email, string Password) :  IRequest<string>;

    public class Handler : IRequestHandler<Command, string>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _identityService.LoginAsync(request.Email, request.Password);
        }
    }
}