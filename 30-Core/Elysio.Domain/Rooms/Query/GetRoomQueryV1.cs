using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Rooms.Query;

public class GetRoomQueryV1 : IRequest<RoomDTO>
{
    public Guid Id { get; set; }
}

public class GetRoomQueryV1Validator
    : AbstractValidator<GetRoomQueryV1>
{
    public GetRoomQueryV1Validator()
    {
    }
}

public class GetRoomQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetRoomQueryV1, RoomDTO>
{
    async Task<RoomDTO> IRequestHandler<GetRoomQueryV1, RoomDTO>.Handle(
        GetRoomQueryV1 request, CancellationToken cancellationToken)
    {
        var room = await dbContext.Rooms
            .Include(r => r.Agents)
            .FirstOrDefaultAsync(a => a.Id == request.Id);

        if (room == null)
            throw new NotFoundException("Room", request.Id);

        return room.ToDto();
    }
}