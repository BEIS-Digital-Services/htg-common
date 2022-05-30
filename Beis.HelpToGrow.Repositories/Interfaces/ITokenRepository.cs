using System.Collections.Generic;
using System.Threading.Tasks;
using Beis.HelpToGrow.Persistence.Models;

namespace Beis.HelpToGrow.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        Task AddToken(token token);
        Task<token> GetToken(long enterpriseId, long productId);
        Task<List<token>> GetEnterpriseNotRedeemedToken(int unactionedDays, bool reminder1, bool reminder2, bool reminder3);
        Task UpdateReminderStatus(long tokenId, bool reminder1, bool reminder2, bool reminder3);
        Task<token> GetTokenByEnterpriseId(long enterpriseId);
        Task<token> UpdateToken(token token);
        token GetTokenByTokenCode(string tokenCode);
    }
}