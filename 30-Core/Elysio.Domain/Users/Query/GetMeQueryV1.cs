using Elysio.Data;
using Elysio.Entities;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Users.Query;

public class GetMeQueryV1 : IRequest<UserDTO>
{
    public string UserEmail { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
}

public class GetMeQueryV1Validator
    : AbstractValidator<GetMeQueryV1>
{
    public GetMeQueryV1Validator()
    {
        RuleFor(a => a.UserEmail)
            .Matches(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")
            .WithMessage("Email invalid par rapport à la regex @\"^[\\w-]+(\\.[\\w-]+)*@([\\w-]+\\.)+[a-zA-Z]{2,7}$\"");

        RuleFor(a => a.Name)
            .NotNull()
            .WithMessage("Error");

        RuleFor(a => a.LastName)
            .NotNull()
            .WithMessage("Error");
    }
}

public class GetMeQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetMeQueryV1, UserDTO>
{
    async Task<UserDTO> IRequestHandler<GetMeQueryV1, UserDTO>.Handle(
        GetMeQueryV1 request, CancellationToken cancellationToken)
    {
        // on récupère l'id de l'utilisateur
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.UserEmail);

        // On le créer si il n'existe pas
        if (user == null)
        {
            user = new User
            {
                Name = request.Name,
                LastName = request.LastName,
                Email = request.UserEmail,
                CreatedAt = DateTimeOffset.UtcNow,
                Id = Guid.NewGuid(),
                LastLoginAt = DateTimeOffset.UtcNow,
            };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        else
            user.LastLoginAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return user.ToDto();
    }
}