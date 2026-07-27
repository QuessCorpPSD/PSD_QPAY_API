using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.ARKnockOff
{
    public interface IARKnockOffRepository
    {
        Task<string> SaveARDetails(string xml);
    }
}
