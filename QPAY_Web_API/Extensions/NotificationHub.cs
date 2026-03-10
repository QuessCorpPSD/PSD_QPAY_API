using Microsoft.AspNetCore.SignalR;
using QPay.BAL.IRepository.Invoice;
using System.Security.Claims;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;

namespace QPay.API.Extensions
{
    public class NotificationHub :Hub
    {
        private readonly IInvoiceRepository _iinvoice;
        public NotificationHub(IInvoiceRepository _iinvoice) { 
        this._iinvoice = _iinvoice;
        }
        public async Task SendGridUpdate()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var dashboard = await _iinvoice.BillingDashboard(Convert.ToInt32(userId), "S");
            
            await Clients.All.SendAsync("GridUpdated", dashboard);
        }
    }
}
