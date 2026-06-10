using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Admin
{
    public interface ICompanyPermissionRepository
    {
        Task<DataSet> GetEntityZoneEmployeeId();

        Task<DataSet> LoadCompany(int? BusinessUnitNameId, int? BusinessZonenName);
        Task<DataSet> CreateUpdateDelete(string xml, int createdBy, string mode);

        Task<DataSet> Search(int? Userid, int? Businessunitnameid, int? CompanyPermissionId);
        Task<DataSet> Editdetails(int? Userid, int? Businessunitnameid, int? CompanyPermissionId);

    }
}
