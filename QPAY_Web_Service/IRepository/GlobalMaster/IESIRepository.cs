using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static QPay.UI.Models.GlobalMaster.ESIClass;


namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface IESIRepository
    {
        Task<List<EsiBlockUI>> GetBlocks();
        Task<List<EsiMonthsUI>> GetMonths();
        Task<DataSet> GetEsiblockSearch(string EffectiveDate);
        Task<DataSet> GetEsiblockExporttoExcel(string EffectiveDate);
        Task<EsiResponse> CreateUpdateDeleteEsiblock(EsiblockRequest request);
        Task<List<PaycodeUI>> GetPaycodes();
        Task<List<EsiStateUI>> GetStates();
        Task<List<EsiCityUI>> GetCity(int StateId);
        Task<List<EsiCriteriaTypeUI>> GetCriteriaType();
        Task<DataSet> GetEsiLocationSlabSearch(EsiLocationSlabSearchRequest request);
        Task<DataSet> GetEsiLocationSlabExporttoExcel(EsiLocationSlabSearchRequest request);
        Task<EsiResponse> CreateUpdateDeleteEsiLocationSlab(EsiLocationSlabRequest request);
        Task<DataSet> GetEsiSlabSearch(EsiSlabSearchRequest request);
        Task<DataSet> GetEsiSlabExporttoExcel(EsiSlabSearchRequest request);
        Task<EsiResponse> CreateUpdateDeleteEsiSlab(EsiSlabRequest request);
    }

}
