using ExpensesData.Context;
using ExpensesData.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Bot.Repositories;
public class RoomsRepository
{
    public readonly ExpensesDbContext _context;
    public RoomsRepository(ExpensesDbContext context)
    {
        _context = context;
    }

    public async Task<Room?> GetRoomByChatId(long chatId)
    {
        return await _context.Rooms.FindAsync(chatId);
    }

    public async Task AddRoomAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRoom(Room room)
    {
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }

    public async Task<Room> GetRoomByName(string name)
    {
        return await _context.Rooms.FirstOrDefaultAsync(r => r.Name == name);

    }
}
