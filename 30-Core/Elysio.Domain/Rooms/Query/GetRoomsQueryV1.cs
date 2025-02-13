using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Rooms.Query;

public class GetRoomsQueryV1 : IRequest<IReadOnlyList<RoomDTO>>
{
}

public class GetRoomsQueryV1Validator
    : AbstractValidator<GetRoomsQueryV1>
{
    public GetRoomsQueryV1Validator()
    {
    }
}

public class GetRoomsQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetRoomsQueryV1, IReadOnlyList<RoomDTO>>
{
    async Task<IReadOnlyList<RoomDTO>> IRequestHandler<GetRoomsQueryV1, IReadOnlyList<RoomDTO>>.Handle(
        GetRoomsQueryV1 request, CancellationToken cancellationToken)
    {
        var rooms = dbContext.Rooms;
        return await rooms.Select(r => r.ToDto()).ToListAsync();
    }
}