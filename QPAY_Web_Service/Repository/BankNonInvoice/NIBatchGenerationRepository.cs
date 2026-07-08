using Dapper;
using Microsoft.Extensions.Configuration;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.Word;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.DAL.Repository;
using QPay.UI.Models.BankNonInvoice;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using QPay.BAL.IRepository.BankNonInvoice;

namespace QPay.BAL.Repository.BankNonInvoice
{
    public class NIBatchGenerationRepository : INIBatchGenerationRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public NIBatchGenerationRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        #region BatchTypeLoad start
        public List<CommonDropDown> GetBatchTypeList(int UserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "NIBatchTypeList");
            parameters.Add("@CreatedBy", UserId);

            var res = this._dbRepository.GetItemsAsync("Proc_BatchType_Load", parameters).Result;
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<CommonDropDown>>(res) ?? new List<CommonDropDown>();
            }

            return new List<CommonDropDown>();
        }


        public DataSet GetTemplate(string Flag, int UserId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = Flag,
                ["@CreatedBy"] = UserId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Manage_BatchTemplate", parameters);
        }

        #endregion BatchTypeLoad end

        #region BatchGenerate start
        public DataSet GetSalaryreleaseProcessdata(string BatchType, int EntityId, int BatchCreationType, int Status, int UserId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "Search",
                ["@BatchType"] = BatchType,
                ["@BatchCreationTypes"] = BatchCreationType,
                ["@EntityId"] = EntityId,
                ["@Status"] = Status,
                ["@CreatedBy"] = UserId

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Manage_NIBatchGeneration", parameters);
        }

        public DataSet GetSalaryreleaseProcessExport(string BatchType, int EntityId, int BatchCreationType, int Status, int UserId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "Export",
                ["@BatchType"] = BatchType,
                ["@BatchCreationTypes"] = BatchCreationType,
                ["@EntityId"] = EntityId,
                ["@Status"] = Status,
                ["@CreatedBy"] = UserId

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Manage_NIBatchGeneration", parameters);
        }

        public async Task<List<BulkUploadErrorMessage>> BatchGenerate(NIBatchGenerate payload)
        {
            const string storedProcedure = "Proc_Manage_NIBatchGeneration";

            var parameter = new DynamicParameters();

            string xml = ConvertWithDynamicRoot(payload.BatchList, "BatchGenerationResponse", "BatchGenerationResponse");

            parameter.Add("@Action", "BatchGenerate");
            parameter.Add("@BatchType", payload.BatchType);
            parameter.Add("@BatchCreationTypes", payload.batchCreationTypes);
            parameter.Add("@EntityId", payload.Entity_id);
            parameter.Add("@CreatedBy", payload.UserId);
            parameter.Add("@xmlInput", xml);

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<BulkUploadErrorMessage>
                {
                    new BulkUploadErrorMessage
                    {
                        Validation = "Invalid response"
                    }
                  };
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<BulkUploadErrorMessage>>(res);
                return list?.ToList() ?? new List<BulkUploadErrorMessage>();
            }
            catch (JsonException ex)
            {

                return new List<BulkUploadErrorMessage>
                  {
                    new BulkUploadErrorMessage
                      {
                        Validation = ex.Message
                      }
                 };
            }
        }

        public async Task<List<BulkUploadErrorMessage>> Rejectgroup(string BatchType, int Salary_Process_Initiate_detail_Id, int UserId)
        {
            const string storedProcedure = "Proc_Manage_NIBatchGeneration";

            var parameter = new DynamicParameters();

            parameter.Add("@Action", "Reject");
            parameter.Add("@BatchType", BatchType);
            parameter.Add("@CreatedBy", UserId);
            parameter.Add("@Salary_Process_Initiate_detail_Id", Salary_Process_Initiate_detail_Id);

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<BulkUploadErrorMessage>
                {
                    new BulkUploadErrorMessage
                    {
                        Validation = "Invalid response"
                    }
                  };
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<BulkUploadErrorMessage>>(res);
                return list?.ToList() ?? new List<BulkUploadErrorMessage>();
            }
            catch (JsonException ex)
            {

                return new List<BulkUploadErrorMessage>
                  {
                    new BulkUploadErrorMessage
                      {
                        Validation = ex.Message
                      }
                 };
            }
        }
        public List<EntityMasterNI> EntityListbg(int UserId)
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@Createdby", UserId);         

            var res = this._dbRepository.GetItemsAsync("sp_GetAllNonInvoiceBusinessUnits", parameters).Result;
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<EntityMasterNI>>(res) ?? new List<EntityMasterNI>();
            }

            return new List<EntityMasterNI>();
        }
        public List<CommonGenModel> BatchCreationTypelist(int UserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Description", null);
            parameters.Add("@Action", "BATCH_CREATION_TYPE_BNI");
            parameters.Add("@CreatedBy", UserId);

            var res = this._dbRepository.GetItemsAsync("USP_CommonDropDowns", parameters).Result;
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<CommonGenModel>>(res) ?? new List<CommonGenModel>();
            }

            return new List<CommonGenModel>();
        }

        public async Task<List<SatausErrorMessage>> UploadCollectionStatus(IFormFile file, [FromForm] string BatchType, [FromForm] int UserId)
        {
            SatausErrorMessage ResultMessage = new SatausErrorMessage();
            //var ResultMessage = "";
            var result = "";

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["SalaryReleaseKey"].ToString(), "NICollectionStatus");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"CollectionStatus_{UserId}_{datePrefix}{extension}";

                var filePath = Path.Combine(uploadsFolder, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(filePath);
                ds.Tables[0].TableName = "Table";
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    result = "Excel sheet is empty or not formatted correctly.";

                    // Wrap it into a list of ErrorMessage
                    var errorList = new List<SatausErrorMessage>
                    {
                         new SatausErrorMessage { Error_Message = result }
                   };
                    return errorList;
                }

                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();
                using var xmlWriter2 = new StringWriter();

                //ds.Tables.Add(dtToSerilize.Copy());
                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                // dscolumns.WriteXml(xmlWriter2, XmlWriteMode.IgnoreSchema);              

                string xmlInput = xmlWriter.ToString();
                // string xmlInput2 = xmlWriter2.ToString();

                string storeProcedure = "Proc_Manage_BatchSalaryReleaseStatus";
                var parameters = new DynamicParameters();

                parameters.Add("@xmlInput", xmlInput);
                parameters.Add("@CreatedBy", UserId);
                parameters.Add("@Action", "UtrUpload");
                parameters.Add("@BatchType", BatchType);


                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (string.IsNullOrWhiteSpace(res))
                {
                    return new List<SatausErrorMessage>(); // return empty object if no result
                }

                try
                {
                    var list = JsonConvert.DeserializeObject<List<SatausErrorMessage>>(res);
                    return list?.ToList() ?? new List<SatausErrorMessage>();
                }
                catch (JsonException ex)
                {
                    return new List<SatausErrorMessage>();
                }

            }
            else
            {
                result = "Excel sheet is empty or not formatted correctly.";

                // Wrap it into a list of ErrorMessage
                var errorList1 = new List<SatausErrorMessage>
                    {
                         new SatausErrorMessage { Error_Message = result }
                   };
                return errorList1;
            }


        }
        #endregion BatchGenerate end

        #region Salaryreleaseprocess start
        public List<BatchList> GetSRPBatchList(string BatchType, int UserId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "BatchList");
            parameters.Add("@BatchType", BatchType);
            parameters.Add("@Createdby", UserId);

            var res = this._dbRepository.GetItemsAsync("Proc_Manage_NIBatchSalaryReleaseProcess", parameters).Result;
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<BatchList>>(res) ?? new List<BatchList>();
            }

            return new List<BatchList>();
        }
        public DataSet GetSRPBatchData(string BatchType, string BatchId, int UserId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "Search",
                ["@BatchType"] = BatchType,
                ["@BatchId"] = BatchId,
                ["@CreatedBy"] = UserId

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Manage_NIBatchSalaryReleaseProcess", parameters);
        }
        public byte[] BatchIntitiate(string BatchType, string BatchId, int UserId)
        {
            DataSet ds = new DataSet();

            var parameters = new Dictionary<string, object?>
            {
                ["@Batch_id"] = BatchId

            };

            ds = _dbRepository.ExecuteStoredProcedureToDataSetBatchdownload("sp_Salary_Release_Process", parameters, 0);

            byte[] zipstrem = Expert2SalaryproseccZip(ds, BatchType, BatchId);
            return zipstrem;
        }



        public Byte[] Expert2SalaryproseccZip(DataSet dt, string BatchType, string BatchId)
        {
            byte[] fileBytes = null;
            string Messages = "";
            string Entity = "", zipfile = "";

            var controlerNI = "";
            try
            {
                if (BatchType == "Regular")
                {
                    /*
                    for (int i = 0; i <= dt.Tables.Count - 1; i++)
                    {
                        DataTable table = dt.Tables[i];
                        DataColumnCollection columns = table.Columns;
                        if (columns.Contains("Entity"))
                        {
                            Entity = Convert.ToString(dt.Tables[i].Rows[0][6]);

                        }

                    }
                    */
                }

                FileInfo template = null; //new FileInfo(@"E:\\NON-INVOICE BANK UPLOAD FORMATE\ICICI PAY DIRECT.xlsx");
                FileInfo newfile = null;
                ExcelPackage pck = null;

                #region Export code

                int f = 0;
                int x = 0;
                bool Summarycreated = false;
                bool Detailscreated = false;
                string N_F1 = string.Empty;
                int TablesCount = dt.Tables.Count;
                TablesCount = TablesCount - 1;

                List<string> filesToArchive = new List<string>();
                if (dt != null)
                {
                    if (dt.Tables.Count > 0)
                    {
                        for (int i = 0; i <= (dt.Tables[TablesCount].Rows.Count); i++)
                        {
                            if ((dt.Tables[TablesCount].Rows.Count) - 1 >= f && Convert.ToInt64(dt.Tables[TablesCount].Rows[f]["NEFT_Bank_id"]) > 0)
                            {
                                if (((dt.Tables[TablesCount].Rows.Count) - 1 >= f && Convert.ToInt64(dt.Tables[TablesCount].Rows[f]["NEFT_Bank_id"]) == 5)
                                    || ((dt.Tables[TablesCount].Rows.Count) - 1 >= f && Convert.ToInt64(dt.Tables[TablesCount].Rows[f]["NEFT_Bank_id"]) == 23))
                                {
                                    N_F1 = dt.Tables[TablesCount].Rows[f]["N_F1"].ToString();
                                    template = new FileInfo(_configuration["NoninvoiceTemplate"].ToString() + dt.Tables[TablesCount].Rows[f][1].ToString());

                                    if (Entity != "")
                                    {
                                        newfile = new FileInfo(_configuration["NoninvoiceBatch"].ToString() + Entity + '-' + BatchId);

                                    }
                                    else
                                    {
                                        newfile = new FileInfo(_configuration["NoninvoiceBatch"].ToString() + BatchId);
                                    }

                                    if (!Directory.Exists(newfile.ToString()))
                                    {
                                        Directory.CreateDirectory(newfile.ToString());
                                    }
                                    for (int a = 1; a <= Convert.ToInt64(dt.Tables[TablesCount].Rows[f]["SheetCount"]); a++)
                                    {
                                        if (Entity != "")
                                        {
                                            newfile = new FileInfo(_configuration["NoninvoiceBatch"].ToString() + Entity + '-' + BatchId + "\\" + dt.Tables[TablesCount].Rows[f]["New_File_Name"].ToString() + "_" + a + template.Extension);
                                        }
                                        else
                                        {
                                            newfile = new FileInfo(_configuration["NoninvoiceBatch"].ToString() + BatchId + "\\" + dt.Tables[TablesCount].Rows[f]["New_File_Name"].ToString() + "_" + a + template.Extension);


                                        }
                                        Write(dt.Tables[x], newfile.ToString());
                                        filesToArchive.Add(newfile.ToString());
                                        x++;
                                        UploadFilesInSFTP(newfile.ToString(), dt.Tables[TablesCount].Rows[f]["New_File_Name"].ToString() + "_" + a + template.Extension, dt.Tables[TablesCount].Rows[f]["HTH"].ToString(), dt.Tables[TablesCount].Rows[f]["NEFT_Bank_id"].ToString(), a, BatchId, "1");
                                    }
                                    f++;
                                }
                                else
                                {
                                    N_F1 = dt.Tables[TablesCount].Rows[f]["N_F1"].ToString();
                                    template = new FileInfo(_configuration["NoninvoiceTemplate"].ToString() + dt.Tables[TablesCount].Rows[f][1].ToString());
                                    if (Entity != "")
                                    {
                                        newfile = new FileInfo(_configuration["NoninvoiceBatch"].ToString() + Entity + '-' + BatchId);
                                    }
                                    else
                                    {
                                        newfile = new FileInfo(_configuration["NoninvoiceBatch"].ToString() + BatchId);
                                    }
                                    if (!Directory.Exists(newfile.ToString()))
                                    {
                                        Directory.CreateDirectory(newfile.ToString());
                                    }

                                    for (int a = 1; a <= Convert.ToInt64(dt.Tables[TablesCount].Rows[f]["SheetCount"]); a++)
                                    {
                                        if (Entity != "")
                                        {
                                            newfile = new FileInfo(_configuration["NoninvoiceBatch"].ToString() + Entity + '-' + BatchId + "\\" + dt.Tables[TablesCount].Rows[f]["New_File_Name"].ToString() + "_" + a + template.Extension);
                                        }
                                        else
                                        {
                                            newfile = new FileInfo(_configuration["NoninvoiceBatch"].ToString() + BatchId + "\\" + dt.Tables[TablesCount].Rows[f]["New_File_Name"].ToString() + "_" + a + template.Extension);
                                        }
                                        using (pck = new ExcelPackage(newfile, template))
                                        {
                                            ExcelWorksheet ws = pck.Workbook.Worksheets[1];
                                            ws.Cells[dt.Tables[TablesCount].Rows[f]["Starting_Row_No"].ToString()].LoadFromDataTable(dt.Tables[x], false);
                                            filesToArchive.Add(newfile.ToString());
                                            pck.Save();
                                            x++;
                                        }

                                        UploadFilesInSFTP(newfile.ToString(), dt.Tables[TablesCount].Rows[f]["New_File_Name"].ToString() + "_" + a + template.Extension, dt.Tables[TablesCount].Rows[f]["HTH"].ToString(), dt.Tables[TablesCount].Rows[f]["NEFT_Bank_id"].ToString(), a, BatchId, "1");
                                    }
                                    f++;
                                }
                            }
                            else
                            {
                                ExcelWorksheet ws = null;
                                // Int64 Neft_bank_id = Convert.ToInt64(dt.Tables[TablesCount].Rows[f]["NEFT_Bank_id"]);
                                string excelfilename = string.Empty;

                                {

                                    for (int inner = 1; inner <= 3; inner++)
                                    {
                                        if (!Summarycreated)
                                        {
                                            if (Entity != "")
                                            {
                                                newfile = new FileInfo(_configuration["NoninvoiceBatch"].ToString() + Entity + '-' + BatchId + "\\" + string.Format(N_F1, "SUMMARY & EMPLOYEE DETAILS") + ".xlsx");
                                            }
                                            else
                                            {
                                                newfile = new FileInfo(_configuration["NoninvoiceBatch"].ToString() + BatchId + "\\" + string.Format(N_F1, "SUMMARY & EMPLOYEE DETAILS") + ".xlsx");
                                            }
                                            if (newfile.Exists) newfile.Delete();
                                            pck = new ExcelPackage(newfile);

                                            ws = pck.Workbook.Worksheets.Add("SUMMARY");
                                            filesToArchive.Add(newfile.ToString());
                                            Summarycreated = true;
                                            ws.Cells["A1"].LoadFromDataTable(dt.Tables[x], true);
                                            ws.Cells["A1:Z1"].Style.Font.Bold = true;
                                            ws.DefaultColWidth = 25;
                                            x++;
                                        }
                                        else if (!Detailscreated)
                                        {
                                            ws = pck.Workbook.Worksheets.Add("EMPLOYEE DETAILS");
                                            Detailscreated = true;
                                            ws.Cells["A1"].LoadFromDataTable(dt.Tables[x], true);
                                            ws.Cells["A1:Z1"].Style.Font.Bold = true;
                                            ws.DefaultColWidth = 25;
                                            x++;
                                        }
                                        else
                                        {
                                            ws = pck.Workbook.Worksheets.Add("EMPLOYEE BANK WISE SUMMARY");
                                            ws.Cells["A1"].LoadFromDataTable(dt.Tables[x], false);
                                            ws.Cells["A3:C3"].Style.Font.Bold = true;
                                            ws.Cells["A1:C11"].AutoFitColumns();
                                            f++;
                                            x++;
                                        }

                                    }
                                }

                                pck.Save();
                            }
                        }
                        if (Entity != "")
                        {
                            zipfile = _configuration["NoninvoiceBatch"].ToString() + Entity + "-" + BatchId + "\\" + Entity + "-" + BatchId + ".rar";

                        }
                        else
                        {
                            zipfile = _configuration["NoninvoiceBatch"].ToString() + BatchId + "\\" + BatchId + ".rar";
                        }

                        //   string zipfile = _configuration["NoninvoiceBatch"].ToString() + BatchId + "\\" + BatchId + ".rar";
                        if (filesToArchive.Count > 0)
                        {
                            CreatingZip(filesToArchive, zipfile);

                            var basePath = _configuration["NoninvoiceBatch"];

                            var fullPath = Path.Combine(basePath, BatchId, BatchId + ".rar");


                            if (System.IO.File.Exists(fullPath))
                            {
                                fileBytes = System.IO.File.ReadAllBytes(fullPath);
                            }

                            // return File(zipfile, "application/zip", BatchId + ".rar");
                            //return File(zipfile, "application/zip", Entity != "" ? Entity + "-" + BatchId + ".rar" : BatchId + ".rar");
                        }
                        else
                        {
                            Messages = "Templete not exists.";
                        }
                    }
                    else
                    {
                        Messages = "Data not exists.";
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLogException.ErrorLog().LogException("Expert2SalaryproseccZip_Invoice", "Common Controller", ex.Message);
            }

            return fileBytes;

            #endregion Export code
        }

        public void UploadFilesInSFTP(string Filepath, string Filename, string HTH, string NEFT_Bank_id, int FileSno, string BatchId, string IsNoninvoice)
        {

            if (HTH == "YES")
            {
                try
                {
                    DataSet hthds = new DataSet();
                    var parameters = new Dictionary<string, object?>
                    {
                        ["@Filepath"] = "Search",
                        ["@Filename"] = Filename,
                        ["@HTH"] = HTH,
                        ["@NEFT_Bank_id"] = NEFT_Bank_id,
                        ["@FileSno"] = FileSno,
                        ["@BatchId"] = BatchId

                    };
                    hthds = _dbRepository.ExecuteStoredProcedureToDataSetBatchdownload("sp_Salary_Release_Process_Invoice_Manage_HTH", parameters);

                    if (hthds.Tables.Count > 0 && hthds.Tables[0].Rows.Count > 0)
                    {
                        if (hthds.Tables[0].Rows[0]["HTH"].ToString() == "YES")
                        {
                            string destinationpath = hthds.Tables[0].Rows[0]["hthdestinationpath"].ToString();
                            string host = hthds.Tables[0].Rows[0]["hthhost"].ToString();
                            string username = hthds.Tables[0].Rows[0]["hthusername"].ToString();
                            string password = hthds.Tables[0].Rows[0]["hthpassword"].ToString();
                            int port = Convert.ToInt32(hthds.Tables[0].Rows[0]["hthport"]);
                            string hthFilename = hthds.Tables[0].Rows[0]["hthFilename"].ToString();

                            string sourcefile = string.Empty;

                            using (SftpClient client = new SftpClient(host, port, username, password))
                            {
                                client.Connect();
                                client.ChangeDirectory(destinationpath);
                                sourcefile = Filepath;
                                using (FileStream fs = new FileStream(sourcefile, FileMode.Open))
                                {
                                    string newFileName = hthFilename;
                                    // client.BufferSize = 4 * 1024;
                                    client.UploadFile(fs, Path.GetFileName(newFileName));
                                }

                                client.Disconnect();
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    //ErrorLogException.ErrorLog().LogException("UploadFilesInSFTP", "Common Controller", ex.Message);
                }

            }
        }


        public static bool CreatingZip(List<string> filesToArchive, string zipName)
        {
            try
            {
                FileInfo f = new System.IO.FileInfo(zipName);
                if (f.Exists) f.Delete();

                using (ZipArchive newFile = ZipFile.Open(zipName, ZipArchiveMode.Create))
                {
                    foreach (string file in filesToArchive)
                    {
                        //Adds the file to the archive
                        newFile.CreateEntryFromFile(file, (new FileInfo(file)).FullName, System.IO.Compression.CompressionLevel.Optimal);
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLogException.ErrorLog().LogException("CreatingZip", "Common", ex.Message);
                return false;
            }
            return true;
        }

        public static void Write(DataTable dt, string outputFilePath)
        {
            int[] maxLengths = new int[dt.Columns.Count];

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                maxLengths[i] = dt.Columns[i].ColumnName.Length;

                foreach (DataRow row in dt.Rows)
                {
                    if (!row.IsNull(i))
                    {
                        int length = row[i].ToString().Length;

                        if (length > maxLengths[i])
                        {
                            maxLengths[i] = length;
                        }
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(outputFilePath, false))
            {
                // Commented below section for not writing headers to notepad
                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                //    sw.Write(dt.Columns[i].ColumnName.PadRight(maxLengths[i] + 2));
                //}

                //sw.WriteLine();
                int RowLoopCount = 0;
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!row.IsNull(i))
                        {
                            sw.Write(row[i].ToString().PadRight(maxLengths[i]));
                        }
                        else
                        {
                            sw.Write(new string(' ', maxLengths[i]));
                        }
                    }
                    RowLoopCount++;

                    if (RowLoopCount != dt.Rows.Count)
                    {
                        sw.WriteLine();
                    }
                }

                sw.Close();
            }
        }

        #endregion Salaryreleaseprocess end

        #region Download Batch start
        public List<BatchList> GetBatchList(string BatchType, string BatchDate, int UserId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "Search");
            parameters.Add("@BatchType", BatchType);
            parameters.Add("@BatchDate", BatchDate);
            parameters.Add("@Createdby", UserId);

            var res = this._dbRepository.GetItemsAsync("Proc_Manage_NIBatchDownload", parameters).Result;
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<BatchList>>(res) ?? new List<BatchList>();
            }

            return new List<BatchList>();
        }

        #endregion Download Batch end
        public static DataSet ExcelToDataSet(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var dataSet = new DataSet();

            foreach (var worksheet in workbook.Worksheets)
            {
                var dataTable = new DataTable(worksheet.Name);
                bool firstRow = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                        {
                            string columnName = cell.IsEmpty() ? $"Column{cell.Address.ColumnNumber}" : cell.GetValue<string>();
                            dataTable.Columns.Add(columnName);
                        }
                        firstRow = false;
                    }
                    else
                    {
                        var values = row.Cells(1, dataTable.Columns.Count)
                                        .Select(cell => cell.IsEmpty() ? string.Empty : cell.GetValue<string>())
                                        .ToArray();

                        dataTable.Rows.Add(values);
                    }
                }

                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }
        public static string ConvertWithDynamicRoot<T>(IEnumerable<T> list, string rootName, string tableName)
        {
            var root = new XElement(rootName);

            foreach (var item in list)
            {
                var serializer = new XmlSerializer(typeof(T));
                using var writer = new StringWriter();
                serializer.Serialize(writer, item);

                var doc = XDocument.Parse(writer.ToString());
                root.Add(new XElement(tableName, doc.Root.Elements()));
            }

            return root.ToString();
        }


    }
}
