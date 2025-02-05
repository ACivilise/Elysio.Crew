using Elysio.Core.Helpers;
using Elysio.Data;
using Elysio.Entities;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Rooms.Command;

public class CreateRoomCommandV1 : IRequest<RoomDTO>
{
    public string UserEmail { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class CreateRoomCommandV1Validator
    : AbstractValidator<CreateRoomCommandV1>
{
    public CreateRoomCommandV1Validator()
    {
        RuleFor(a => a.UserEmail)
            .Matches(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")
            .WithMessage("Email invalid par rapport à la regex @\"^[\\w-]+(\\.[\\w-]+)*@([\\w-]+\\.)+[a-zA-Z]{2,7}$\"");

        RuleFor(a => a.Name)
            .NotNull();
    }
}

public class CreateRoomCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<CreateRoomCommandV1, RoomDTO>
{
    async Task<RoomDTO> IRequestHandler<CreateRoomCommandV1, RoomDTO>.Handle(
        CreateRoomCommandV1 request, CancellationToken cancellationToken)
    {
        var user = await CommandHelper.ValidateUser(dbContext, request.UserEmail);

        // on crée une conversation associé à cet utilisateur
        var newId = Guid.NewGuid();
        dbContext.Rooms.Add(new Room
        {
            Id = newId,
            Name = request.Name,
            Description = request.Description,
            CreatorId = user.Id,
            CreatedAt = DateTimeOffset.UtcNow
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        var newRoom = await dbContext.Rooms.FirstAsync(c => c.Id == newId);

        return newRoom.ToDto();
    }
}