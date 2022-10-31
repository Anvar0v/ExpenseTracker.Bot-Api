using ExpensesData.Context;
using ExpensesData.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Bot.Repositories;

public class OutlaysRepository
{
    public readonly ExpensesDbContext _context;
    public OutlaysRepository(ExpensesDbContext context)
    {
        _context = context;
    }
    public async Task AddOutlaysAsync(Outlay outlay)
    {
        await _context.Outlays.AddAsync(outlay);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOutlayByRoomName(string roomName,Outlay outlay)
    {
       var room =  await _context.Outlays.FirstOrDefaultAsync(r => r.Room.Name == roomName);
        if (room is not null)
        _context.Outlays.Update(outlay);
        await _context.SaveChangesAsync();
    }

    public async Task CalculateOytlaysAsync()
    {
        await _context.Outlays.SumAsync(outlay => outlay.Cost);
    }
}
