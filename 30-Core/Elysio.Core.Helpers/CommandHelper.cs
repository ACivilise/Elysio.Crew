using Elysio.Data;
using Elysio.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Core.Helpers;

public static class CommandHelper
{
    public static async Task<User> ValidateUser(ApplicationDbContext dbContext, string userEmail)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == userEmail);

        if (user == null)
            throw new Exception($"L'utilisateur {userEmail} n'existe pas");

        return user;
    }
}