using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.IBankNonInvoice;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.BankNonInvoice
{
    public class PartialSalaryReleaseStatusRepository: IPartialSalaryReleaseStatusRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public PartialSalaryReleaseStatusRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }
    }
}
