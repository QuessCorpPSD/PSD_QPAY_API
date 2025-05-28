using QPay.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
   public class AllotmentLotStatusUI
    {
        private bool _QC_Verified_Status;

        public bool QC_Verified_Status
        {
            get => _QC_Verified_Status;
            set
            {
                _QC_Verified_Status = value;
            }
        }

        private bool _Report_Status;

        public bool Report_Status
        {
            get => _Report_Status;
            set
            {
                _Report_Status = value;
            }
        }

        private bool _Customer_Confirmation_Status;

        public bool Customer_Confirmation_Status
        {
            get => _Customer_Confirmation_Status;
            set
            {
                _Customer_Confirmation_Status = value;
            }
        }

        private bool _Invoice_Status;

        public bool Invoice_Status
        {
            get => _Invoice_Status;
            set
            {
                _Invoice_Status = value;
            }
        }

        public FileResponse fileResponse { get; set; }
        public PayRegisterResponse QzoneUpdateStatus { get; set; }
    }
}
