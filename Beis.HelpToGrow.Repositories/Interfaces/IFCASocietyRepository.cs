using System.Collections.Generic;
using System.Threading.Tasks;
using Beis.Htg.VendorSme.Database.Models;

namespace Beis.HelpToGrow.Repositories.Interfaces
{
    public interface IFCASocietyRepository
    {
        Task<int> GetFCASocietiesCount();
        Task AddSocieties(IEnumerable<fcasociety> fcaSocieties);
        Task<fcasociety> GetFCASociety(string fullRegistrationNumber);
        Task<fcasociety> GetFCASocietyByNumber(string societyNumber);
    }
}