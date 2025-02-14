using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Rooms.Query;

public class GetRoomsQueryV1 : IRequest<IReadOnlyList<RoomDTO>>
{
    public bool IncludeAgents { get; set; }
}

public class GetRoomsQueryV1Validator : AbstractValidator<GetRoomsQueryV1>
{
    public GetRoomsQueryV1Validator()
    {
    }
}

public class GetRoomsQueryV1Handler(ApplicationDbContext dbContext) : IRequestHandler<GetRoomsQueryV1, IReadOnlyList<RoomDTO>>
{
    async Task<IReadOnlyList<RoomDTO>> IRequestHandler<GetRoomsQueryV1, IReadOnlyList<RoomDTO>>.Handle(
        GetRoomsQueryV1 request, CancellationToken cancellationToken)
    {
        var query = dbContext.Rooms
            .AsNoTracking();
        if (request.IncludeAgents)
            query = query.Include(r => r.Agents);
        var rooms = await dbContext.Rooms
            .Include(r => r.Agents)
            .ToListAsync(cancellationToken);

        return rooms.Select(r => r.ToDto()).ToList();
    }
}