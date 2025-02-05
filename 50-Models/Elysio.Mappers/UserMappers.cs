using Elysio.Entities;
using Elysio.Models.DTOs;

namespace Elysio.Mappers;

public static class UserMappers
{
    public static UserDTO ToDto(this User user)
    {
        if (user is null)
            return default;

        return new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
        };
    }
}