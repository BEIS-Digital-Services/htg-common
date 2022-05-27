using System.Threading.Tasks;
using Beis.Htg.VendorSme.Database.Models;

namespace Beis.HelpToGrow.Repositories.Interfaces
{
    public interface IIndesserResponseRepository
    {
        Task<indesser_api_call_status> AddIndesserResponse(indesser_api_call_status indesserApiCallStatus);
    }
}