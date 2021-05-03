using FinalYearProject.Extensions;
using FinalYearProject.Models;
using Plugin.CloudFirestore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalYearProject.Services.Database.Reports
{
    public class MessageReportDBService : BaseDBService, IMessageReportDBService
    {
        private static readonly string ReportCollectionName = "Reports";

        public async Task AddMessageReportAsync(MessageReport messageReport,
                                                string groupId,
                                                string newId = null)
        {
            messageReport.ThrowIfNull(nameof(messageReport));
            groupId.ThrowIfNull(nameof(groupId));

            await AddAsync(messageReport, newId, ReportCollectionReference(groupId));
        }

        public async Task<IList<MessageReport>> GetMessageReportsAsync(string groupId)
        {
            groupId.ThrowIfNull(nameof(groupId));

            return await GetAllAsync<MessageReport>(ReportCollectionReference(groupId));
        }

        public async Task RemoveMessageReportsAsync(string groupId, IEnumerable<string> reportIds)
        {
            groupId.ThrowIfNull(nameof(groupId));
            reportIds.ThrowIfNull(nameof(reportIds));

            foreach (var reportId in reportIds)
            {
                await DeleteAsync<MessageReport>(reportId, ReportCollectionReference(groupId));
            }
        }

        private ICollectionReference ReportCollectionReference(string groupId)
        {
            return GetBaseCollectionReference<Models.Group>()
                .Document(groupId)
                .Collection(ReportCollectionName);
        }
    }
}
