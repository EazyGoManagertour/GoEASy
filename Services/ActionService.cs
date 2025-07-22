using GoEASy.Models;
using GoEASy.Repositories;
using Microsoft.EntityFrameworkCore;
using ActionModel = GoEASy.Models.Action; // 👈 alias

public class ActionService : IActionService
{
    private readonly GoEasyContext _context;

    public ActionService(GoEasyContext context)
    {
        _context = context;
    }

    public async Task AddActionAsync(ActionModel action)
    {
        await _context.Actions.AddAsync(action);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ActionModel>> GetAllActionsAsync()
    {
        var actions = await _context.Actions.ToListAsync();
        return actions;
    }
}
