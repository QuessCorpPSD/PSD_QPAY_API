using QPay.UI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Utilities
{
    public class WriteXmlFromExcel
    {
        public static string ReadFileFromExcel(string Filepath, string UploadFileName = "NotEention")
        {
            DataSet ds = new DataSet();
            string fileExtension = Path.GetExtension(Filepath).ToUpper();
            string filename = Path.GetFileName(Filepath);
            string XmlDoc = string.Empty;
            string[] ExcelColumnNames;
            string[] ActualColumnNames;
            bool equal = true;
            try
            {
                if (fileExtension == ".XLS" || fileExtension == ".XLSX")
                {
                    string excelConnectionString = string.Empty;
                    //connection String for xls file format.

                    switch (fileExtension)
                    {
                        case ".XLS":
                            {
                                excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Filepath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                                break;
                            }
                        case ".XLSX":
                            {
                                excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Filepath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                                break;
                            }
                    }
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    System.Data.DataTable dt = new System.Data.DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        //return null;
                    }
                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }

                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                    string query = string.Format("Select * from [{0}]", excelSheets[0]);

                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                    System.Data.DataTable dtToSerilize = new System.Data.DataTable();
                    dtToSerilize = ds.Tables[0];

                    
                    switch (UploadFileName)
                    {
                        case "OTHERINCOME":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUploadLOtherIncome_With_TDS_ESI_ERESI_Columns();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUploadLOtherIncome_With_OUT_TDS_ESI_ERESI_Columns();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;

                        case "SalaryReleaseApproveTemplate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_SalaryReleasePendingApprove();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_SalaryReleasePendingApprove();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;

                        case "AccountNumberRequestTemplate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_AccountNumberRequest();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_AccountNumberRequest();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        // Start by Anant on 17-Oct-18 for PartialHold Salary Release
                        case "PartialHoldSalaryReleaseTemplate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_PartialHoldSalaryRelease();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_PartialHoldSalaryRelease();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        // End by Anant on 17-Oct-18 for PartialHold Salary Release
                        // Start by Anant on 26-Nov-18 for Bonus_Release
                        case "BonusReleaseTemplate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BonusRelease();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BonusRelease();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        // End by Anant on 26-Nov-18 for Bonus_Release
                        // CancelInvoiceMappingTemplate
                        case "CancelInvoiceMappingTemplate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_CancelInvoiceMapping();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_CancelInvoiceMapping();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        //BankInvoiceManualUploadTemplate
                        case "BankInvoiceManualUploadTemplate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BankInvoiceManualUpload();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BankInvoiceManualUpload();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "UploadCompProvBenifitsTemplate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_CompanyProvidedUpload();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_CompanyProvidedUpload();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        //ManualBatchCreationTemplate
                        case "ManualBatchCreationTemplate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_ManualBatchCreation();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_ManualBatchCreation();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        //BatchRejectionTemplate
                        case "BatchRejectionTemplate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BatchRejection();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BatchRejection();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "TaxRemittanceDetail"://TaxRemittanceDetail Added By Vijay on 22Feb2019
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_TaxRemittanceColumns();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_TaxRemittanceColumns();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "BadDebtUpdate":// Added By Vijay on 28/Feb/2019
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BadDebtUpdateColumns();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BadDebtUpdateColumns();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "BeneficiaryUpdate":// Added By Vijay on 3/Mar/2019
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BeneficiaryUpdateColumns();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BeneficiaryUpdateColumns();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;

                        case "LoanPreClosureTemplate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_LoanPreClosure();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_LoanPreClosure();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;

                        case "UploadPayFrequencyGroup": //Added By Rudra 
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_PayfrequencygroupColumns();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_PayfrequencygroupColumns();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;

                        case "UploadAccruals": // Method for uploading MIS accrual details. Added By Vijay on 11/June/2019
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetUploadAccrualsColumns();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetUploadAccrualsColumns();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "MultipleClientInvoiceMapping":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_MultipleClientInvoiceMapping();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_MultipleClientInvoiceMapping();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "EmployeeTransferRequest":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_EmployeeTransferRequest();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_EmployeeTransferRequest();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "EmployeeTransferApproval":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_EmployeeTransferApproval();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_EmployeeTransferApproval();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;

                        case "BNIFinanceHoldTemplate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BNIFinanceHoldTemplate();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BNIFinanceHoldTemplate();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        //BNIFinanceReleaseTemplate
                        case "BNIFinanceReleaseTemplate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BNIFinanceReleaseTemplate();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BNIFinanceReleaseTemplate();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;

                        case "CreditNoteAdjustment":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_CreditNoteAdjustment();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_CreditNoteAdjustment();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;

                        case "MapNameChanges":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_MapNameChanges();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_MapNameChanges();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "OtherincomeMapNameChanges":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_OtherincomeMapNameChanges();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_OtherincomeMapNameChanges();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "AttendenceLeaveUpdate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_AttendanceLeaveUpdate();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_AttendanceLeaveUpdate();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "PfCodeCulture":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_PfCodeCulture();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_PfCodeCulture();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;

                        //Rudra
                        case "UanActiveStatus":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_UanActiveStatus();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_UanActiveStatus();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "ECRDataUpload":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_ECRDataUpload();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_ECRDataUpload();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        //Rudra
                        case "PanRemarks":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_PanRemarks();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_PanRemarks();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        //Rudra
                        case "TiscActiveStatus":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_TiscActiveStatus();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_TiscActiveStatus();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;

                        //Rudra
                        case "EsiSubCode":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_Esisubcode();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_Esisubcode();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;

                        //Rudra
                        case "ECREsiSubCode":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_EsisubcodeCodeCulture();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_EsisubcodeCodeCulture();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        //Rudra
                        case "ESIECRDataUpload":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_ESIECRDataUpload();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_ESIECRDataUpload();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "PfEcrChallanUpload":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_PfEcrChallanUpload();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_PfEcrChallanUpload();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "AadhaarAuthenticateStatus":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_AadhaarAuthenticateStatus();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_AadhaarAuthenticateStatus();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "SubmissionDateUpdate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_SubmissionDateUpdate();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_SubmissionDateUpdate();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "UploadPoCulture":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_PoCultureDateUpdate();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_PoCultureDateUpdate();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "WBSUpdationTemplate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_WBSUpdation();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_WBSUpdation();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "PayRegisterUploadDeletionTemplate":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_PayRegisterUploadDeletion();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_PayRegisterUploadDeletion();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        case "ProfomaImport":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_ProfomaImport();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_ProfomaImport();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        // satart by praveen kumar on 25-Mar-24 for DBT Hold Salary
                        case "DBTHoldEmployeeSalary":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_DBTHoldEmployeeSalary();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_DBTHoldEmployeeSalary();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        // End by praveen kumar on 25-Mar-24 for DBT Hold Salary
                        // satart by praveen kumar on 25-Mar-24 for DBT Hold Salary Release
                        case "DBTHoldSalaryRelease":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_DBTHoldSalaryRelease();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_DBTHoldSalaryRelease();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        // End by praveen kumar on 25-Mar-24 for DBT Hold Salary Release
                        // satart by praveen kumar on 25-Mar-24 for BlockCNAmountHold
                        case "BlockCNAmountHold":
                            ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                            ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BlockCNAmountHold();
                            equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            if (!equal)
                            {
                                ExcelColumnNames = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                                ActualColumnNames = BulkUploadTemplateColumns.GetBulkUpload_BlockCNAmountHold();
                                equal = ExcelColumnNames.Except(ActualColumnNames).Count() == 0 && ActualColumnNames.Except(ExcelColumnNames).Count() == 0;
                            }
                            break;
                        // End by praveen kumar on 25-Mar-24 for BlockCNAmountHold
                        default:
                            break;
                    }

                    if (equal)
                    {
                        using (StringWriter ms = new StringWriter())
                        {
                            dtToSerilize.WriteXml(ms);
                            XmlDoc = ms.ToString().Replace("_x0020_", "").Replace("_x0028_", "").Replace("_x002F_", "").Replace("_x0029_", "").Replace("_x0027_", "").Replace("_x003A_", "").Replace("_x0023_", "");
                        }
                    }
                    else
                    {
                        XmlDoc = "Columns Name are not matching";
                    }
                }
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
            return XmlDoc;
        }

     

    }
}
