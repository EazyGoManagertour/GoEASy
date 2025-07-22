using GoEASy.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public interface IRuleService
    {
        Task<List<Rule>> GetAllRulesAsync();
        Task<Rule> GetRuleByIdAsync(int id);
        Task AddRuleAsync(Rule rule);
        Task UpdateRuleAsync(Rule rule);
        Task DeleteRuleAsync(int id);
        Task UpdateRulePermissionsAsync(int ruleId, List<string> slugs);
    }
}