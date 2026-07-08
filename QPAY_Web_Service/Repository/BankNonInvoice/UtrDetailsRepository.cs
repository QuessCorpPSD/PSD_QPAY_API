using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.BankNonInvoice;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.BankNonInvoice
{
    public class UtrDetailsRepository :IUtrDetailsRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public UtrDetailsRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }
        public async Task<FileContentResult> GetutrDetailDownload(int Company_Id, int Pay_Period_Id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = Company_Id,
                ["@PayPeriodID"] = Pay_Period_Id
            };

            var ds = _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Proc_NetPaySummary_BNI",
                parameters,
                1500
            );

            DataTable dt = ds.Tables[0];

            StringBuilder sb = new StringBuilder();

            foreach (DataColumn col in dt.Columns)
            {
                sb.Append(col.ColumnName + "\t");
            }

            sb.AppendLine();

            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    sb.Append(item?.ToString() + "\t");
                }

                sb.AppendLine();
            }

            return new FileContentResult(
                Encoding.UTF8.GetBytes(sb.ToString()),
                "application/vnd.ms-excel"
            )
            {
                FileDownloadName = "UtrDetail.xls"
            };
        }

        public DataSet NetPaysummaryNI(int Company_Id, int Pay_Period_Id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@PayPeriodID"] = Pay_Period_Id,
                //["@QZoneUserName"] = QZoneUserName,
                ["@CompanyId"] = Company_Id,


            };
            DataSet ds = new DataSet();
            ds = _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_NetPaySummary_BNI", parameters, 0);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given company and pay period.");
            }


        }
    }

}
