using ExpensesData.Context;
using ExpensesData.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Bot.Repositories;
public class UsersRepository
{
    public readonly ExpensesDbContext _context;
    public UsersRepository(ExpensesDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByChatId(long chatId)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.ChatId == chatId);
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
         _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
