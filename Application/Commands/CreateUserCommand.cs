using Domain.Models;
using MediatR;
using Persistence;

namespace Application.Commands;

public class CreateUserCommand
{
    public record UserInfoCommand : IRequest
    {
        public string Name { get; set; }
    }

    public class Handler : IRequestHandler<UserInfoCommand>
    {
        private readonly CurrentMessagesNetContext _context;

        public Handler(CurrentMessagesNetContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(UserInfoCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Name = request.Name
            };

            await _context.Users.AddAsync(user);

            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
            {
                return Unit.Value;
            }
            
            throw new Exception("No se pudo crear el usuario");
        }
    }
}