using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.BankNonInvoice
{
    public interface IUtrDetailsRepository
    {
        Task<FileContentResult> GetutrDetailDownload(int Company_Id, int Pay_Period_Id);

        DataSet NetPaysummaryNI(int Company_Id, int Pay_Period_Id);

    }
}
