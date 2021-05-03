using FinalYearProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalYearProject.Services.Database.Reports
{
    public interface IMessageReportDBService
    {
        Task AddMessageReportAsync(MessageReport messageReport, string groupId, string newId = null);
        Task<IList<MessageReport>> GetMessageReportsAsync(string groupId);
        Task RemoveMessageReportsAsync(string groupId, IEnumerable<string> reportIds);
    }
}