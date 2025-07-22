using ActionModel = GoEASy.Models.Action; // 👈 alias để tránh trùng
using System.Threading.Tasks;

public interface IActionService
{
    Task AddActionAsync(ActionModel action);
    Task<List<ActionModel>> GetAllActionsAsync();
}
