using GoEASy.Repositories;
using RuleModel = GoEASy.Models.Rule; // ALIAS tránh trùng System.Data.Rule
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public class RuleService : IRuleService
    {
        private readonly IGenericRepository<RuleModel> _ruleRepo;

        public RuleService(IGenericRepository<RuleModel> ruleRepo)
        {
            _ruleRepo = ruleRepo;
        }

        public async Task<List<RuleModel>> GetAllRulesAsync()
        {
            var rules = await _ruleRepo.GetAllAsync();
            return rules.ToList();
        }

        public async Task<RuleModel> GetRuleByIdAsync(int id)
        {
            return await _ruleRepo.GetByIdAsync(id);
        }

        public async Task AddRuleAsync(RuleModel rule)
        {
            await _ruleRepo.AddAsync(rule);
            await _ruleRepo.SaveAsync();
        }

        public async Task UpdateRuleAsync(RuleModel rule)
        {
            _ruleRepo.Update(rule);
            await _ruleRepo.SaveAsync();
        }

        public async Task DeleteRuleAsync(int id)
        {
            var rule = await _ruleRepo.GetByIdAsync(id);
            if (rule != null)
            {
                _ruleRepo.Delete(rule);
                await _ruleRepo.SaveAsync();
            }
        }

        public async Task UpdateRulePermissionsAsync(int ruleId, List<string> slugs)
        {
            var rule = await _ruleRepo.GetByIdAsync(ruleId);
            if (rule != null)
            {
                rule.ListRuleSlug = string.Join(",", slugs);
                _ruleRepo.Update(rule);
                await _ruleRepo.SaveAsync();
            }
        }


    }
}


