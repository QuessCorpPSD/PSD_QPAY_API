using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Invoice
{
    public class EinvoiceModel
    {
        public int? Invoice_Id { get; set; }
        public string Invoice_Number { get; set; } = "";
        public int? Company_Id { get; set; }
        public int? Cost_Center_Mapping_Id { get; set; }
        public int? Pay_Period_Id { get; set; }
        public int Invoice_Type_Id { get; set; }
        public DateTime? Invoice_Date { get; set; }
        public Decimal Amount { get; set; }
        public int? StateId { get; set; }
        public int? InvoicingStateId { get; set; }
        public Decimal CGST_Percentage { get; set; }
        public Decimal SGST_Percentage { get; set; }
        public Decimal UTGST_Percentage { get; set; }
        public Decimal IGST_Percentage { get; set; }
        public string Status { get; set; } = "";
        public Decimal? CGST_Amount { get; set; }
        public Decimal? SGST_Amount { get; set; }
        public Decimal? UTGST_Amount { get; set; }
        public Decimal? IGST_Amount { get; set; }
        public Decimal? Net_Amount { get; set; }
        public string Company_Code { get; set; } = "";
        public string InvoiceType { get; set; } = "";
        public string Map_Name { get; set; } = "";
        public string State_Name { get; set; } = "";
        public string Pay_Period { get; set; } = "";
        public string Credit_Note_Number { get; set; } = "";
        public Decimal? Credit_Note_Amount { get; set; }
        public DateTime? Credit_Note_Date { get; set; }
        public DateTime? From_Date { get; set; }
        public DateTime? To_Date { get; set; }
        public string DebitNote_No { get; set; } = "";
        public Decimal? Debit_Note_Amount { get; set; }
        public DateTime? DebitNote_Date { get; set; }
        public string DebitNoteType { get; set; } = "";
        public int? DebitNote_Id { get; set; }
        public DateTime? IRN_Date { get; set; }
    }

    public class initiateIRN
    {
        public string[]? invoiceIds { get; set; }
        public int companyId { get; set; }
        public string payPeriod { get; set; } = "";
        public int payPeriodId { get; set; }
        public string userId { get; set; } = "";
    }

    public class BulkInvoices
    {
        public List<int> invoiceIds { get; set; }
    }
    public class InitiateIRN
    {
        public List<int> invoiceIds { get; set; }
        public int? CompanyId { get; set; }
        public int? PayPeriodId { get; set; }
        public string? userId { get; set; }
    }

    public class EInvoice
    {
        public string client_id { get; set; }
        public string client_hash { get; set; }
        public string pan { get; set; }
        public string ip_addr { get; set; }
        public string file_type { get; set; }
        public EInvoice()
        {
            docs = new List<Docs>();
        }
        public List<Docs> docs { get; set; }
    }
    public class Docs
    {
        public Docs()
        {
            TranDtls = new TranDtls();
            DocDtls = new DocDtls();
            SellerDtls = new SellerDtls();
            BuyerDtls = new BuyerDtls();
            DispDtls = new DispDtls();
            ShipDtls = new ShipDtls();
            ItemList = new List<ItemList>();
            ValDtls = new ValDtls();
            PayDtls = new PayDtls();
            RefDtls = new RefDtls();
            AddlDocDtls = new AddlDocDtls();
            ExpDtls = new ExpDtls();
            EwbDtls = new EwbDtls();
            CustomRefs = new CustomRefs();
        }
        public string Version { get; set; }
        public TranDtls TranDtls { get; set; }
        public DocDtls DocDtls { get; set; }
        public SellerDtls SellerDtls { get; set; }
        public BuyerDtls BuyerDtls { get; set; }
        public DispDtls DispDtls { get; set; }
        public ShipDtls ShipDtls { get; set; }
        public List<ItemList> ItemList { get; set; }
        public ValDtls ValDtls { get; set; }
        public PayDtls PayDtls { get; set; }
        public RefDtls RefDtls { get; set; }
        public AddlDocDtls AddlDocDtls { get; set; }
        public ExpDtls ExpDtls { get; set; }
        public EwbDtls EwbDtls { get; set; }
        public CustomRefs CustomRefs { get; set; }
    }
    public class TranDtls
    {
        public string Tran_TaxSch { get; set; }
        public string Tran_SupTyp { get; set; }
        public string Tran_RegRev { get; set; }
        public string Tran_Typ { get; set; }
        public string Tran_Ecmgstin { get; set; }
        public string Tran_IgstOnIntra { get; set; }
    }
    public class DocDtls
    {
        public string Doc_Typ { get; set; }
        public string Doc_No { get; set; }
        public string Doc_Dt { get; set; }
        public string Doc_FY { get; set; }
    }
    public class SellerDtls
    {
        public string Seller_Gstin { get; set; }
        public string Seller_LglNm { get; set; }
        public string Seller_TrdNm { get; set; }
        public string Seller_Addr1 { get; set; }
        public string Seller_Addr2 { get; set; }
        public string Seller_Loc { get; set; }
        public int Seller_Pin { get; set; }
        public int Seller_Stcd { get; set; }
        public long Seller_Ph { get; set; }
        public string Seller_Em { get; set; }
    }
    public class BuyerDtls
    {
        public string Buyer_GSTIN { get; set; }
        public string Buyer_LglNm { get; set; }
        public string Buyer_TrdNm { get; set; }
        public string Buyer_POS { get; set; }
        public string Buyer_Addr1 { get; set; }
        public string Buyer_Addr2 { get; set; }
        public string Buyer_Loc { get; set; }
        public int Buyer_Pin { get; set; }
        public int Buyer_Stcd { get; set; }
        public long Buyer_Ph { get; set; }
        public string Buyer_Em { get; set; }
    }
    public class DispDtls
    {
        public string Dispatch_Fr_Nm { get; set; }
        public string Dispatch_Fr_Addr1 { get; set; }
        public string Dispatch_Fr_Addr2 { get; set; }
        public string Dispatch_Fr_Loc { get; set; }
        public int Dispatch_Fr_Pin { get; set; }
        public int Dispatch_Fr_Stcd { get; set; }
        public long Dispatch_Fr_Ph { get; set; }
        public string Dispatch_Fr_Em { get; set; }
    }
    public class ShipDtls
    {
        public string Ship_To_Gstin { get; set; }
        public string Ship_To_LglNm { get; set; }
        public string Ship_To_TrdNm { get; set; }
        public string Ship_To_Addr1 { get; set; }
        public string Ship_To_Addr2 { get; set; }
        public string Ship_To_Loc { get; set; }
        public int Ship_To_Pin { get; set; }
        public int Ship_To_Stcd { get; set; }
        public long Ship_To_Ph { get; set; }
        public string Ship_To_Em { get; set; }
    }
    public class ItemList
    {
        public int Item_SlNo { get; set; }
        public string Item_PrdDesc { get; set; }
        public string Item_IsServc { get; set; }
        public string Item_HsnCd { get; set; }
        public string Item_Barcde { get; set; }
        public int Item_Qty { get; set; }
        public string Item_FreeQty { get; set; }
        public string Item_Unit { get; set; }
        public string Item_UnitPrice { get; set; }
        public string Item_TotAmt { get; set; }
        public string Item_Discount { get; set; }
        public string Item_PreTaxVal { get; set; }
        public string Item_AssAmt { get; set; }
        public string Item_GstRt { get; set; }
        public string Item_IgstAmt { get; set; }
        public string Item_CgstAmt { get; set; }
        public string Item_SgstAmt { get; set; }
        public string Item_CesRt { get; set; }
        public string Item_CesAmt { get; set; }
        public string Item_CesNonAdvlAmt { get; set; }
        public string Item_StateCesRt { get; set; }
        public string Item_StateCesAmt { get; set; }
        public string Item_StateCesNonAdvlAmt { get; set; }
        public string Item_OthChrg { get; set; }
        public string Item_TotItemVal { get; set; }
        public string Item_OrdLineRef { get; set; }
        public string Item_OrgCntry { get; set; }
        public string Item_PrdSlNo { get; set; }
        public string Ref11 { get; set; }
        public string Ref12 { get; set; }
        public string Ref13 { get; set; }
        public string Ref14 { get; set; }
        public string Ref15 { get; set; }
        public ItemList()
        {
            AttribDtls = new List<AttribDtls>();
            BchDtls = new List<BchDtls>();
        }
        public List<AttribDtls> AttribDtls { get; set; }
        public List<BchDtls> BchDtls { get; set; }
    }
    public class AttribDtls
    {
        public int Attrib_SlNo { get; set; }
        public string Attrib_Nm { get; set; }
        public string Attrib_Val { get; set; }
    }
    public class BchDtls
    {
        public int Bch_SlNo { get; set; }
        public string Bch_Nm { get; set; }
        public string Bch_ExpDt { get; set; }
        public string Bch_WrDt { get; set; }
    }
    public class ValDtls
    {
        public string Doc_TotVal { get; set; }
        public string Doc_DiscountVal { get; set; }
        public string Doc_AssVal { get; set; }
        public string Doc_IgstVal { get; set; }
        public string Doc_CgstVal { get; set; }
        public string Doc_SgstVal { get; set; }
        public string Doc_CesVal { get; set; }
        public string Doc_CesNonAdvlVal { get; set; }
        public string Doc_StCesVal { get; set; }
        public string Doc_StCesNonAdvlVal { get; set; }
        public string Doc_RndOffAmt { get; set; }
        public string Doc_PreTaxVal { get; set; }
        public string Doc_OthChrgVal { get; set; }
        public string Doc_TotInvVal { get; set; }
        public string Doc_TotInvValFc { get; set; }
    }
    public class PayDtls
    {
        public string Payee_Nm { get; set; }
        public string Payee_AccDet { get; set; }
        public string Payee_Mode { get; set; }
        public string Payee_FinInsBr { get; set; }
        public string Payee_PayTerm { get; set; }
        public string Payee_PayInstr { get; set; }
        public string Payee_CrTrn { get; set; }
        public string Payee_DirDr { get; set; }
        public int Payee_CrDay { get; set; }
        public string Payee_PaidAmt { get; set; }
        public string Payee_PaymtDue { get; set; }
    }
    public class RefDtls
    {
        public string Ref_InvRmk { get; set; }
        public string Ref_InvStDt { get; set; }
        public string Ref_InvEndDt { get; set; }
        public RefDtls()
        {
            PrecDocDtls = new List<PrecDocDtls>();
            ContrDtls = new List<ContrDtls>();
        }
        public List<PrecDocDtls> PrecDocDtls { get; set; }
        public List<ContrDtls> ContrDtls { get; set; }
    }
    public class PrecDocDtls
    {
        public int PrecDoc_SlNo { get; set; }
        public string PrecDoc_PrecInvNo { get; set; }
        public string PrecDoc_PrecInvDt { get; set; }
        public string PrecDoc_OthRefNo { get; set; }
    }
    public class ContrDtls
    {
        public int Contr_SlNo { get; set; }
        public string Contr_RecAdvRefr { get; set; }
        public string Contr_RecAdvDt { get; set; }
        public string Contr_TendRefr { get; set; }
        public string Contr_ContrRefr { get; set; }
        public string Contr_ExtRefr { get; set; }
        public string Contr_ProjRefr { get; set; }
        public string Contr_PORefr { get; set; }
        public string Contr_PORefDt { get; set; }
    }
    public class AddlDocDtls
    {
        public int AddlDoc_SlNo { get; set; }
        public string AddlDoc_URL { get; set; }
        public string AddlDoc_Docs { get; set; }
        public string AddlDoc_Info { get; set; }
    }
    public class ExpDtls
    {
        public string Exp_ShipBNo { get; set; }
        public string Exp_ShipBDt { get; set; }
        public string Exp_Port { get; set; }
        public string Exp_RefClm { get; set; }
        public string Exp_ForCur { get; set; }
        public string Exp_CntCode { get; set; }
        public string Exp_Duty { get; set; }
    }
    public class EwbDtls
    {
        public string Ewb_TransID { get; set; }
        public string Ewb_TransName { get; set; }
        public string Ewb_TransMode { get; set; }
        public string Ewb_Distance { get; set; }
        public string Ewb_TransDocNo { get; set; }
        public string Ewb_TransDocDt { get; set; }
        public string Ewb_VehNo { get; set; }
        public string Ewb_VehType { get; set; }
    }
    public class CustomRefs
    {
        public string Ref01 { get; set; }
        public string Ref02 { get; set; }
        public string Ref03 { get; set; }
        public string Ref04 { get; set; }
        public string Ref05 { get; set; }
        public string Ref06 { get; set; }
        public string Ref07 { get; set; }
        public string Ref08 { get; set; }
        public string Ref09 { get; set; }
        public string Ref10 { get; set; }
    }

    public class InvoiceColors
    {
        public string? label { get; set; }
        public string? color { get; set; }

    }


    public class DownloadRegister
    {
        public int Company_Id { get; set; }
        public string Company_Name { get; set; } = "";
        public string Company_Code { get; set; } = "";
        public int Pay_Period_Id { get; set; }
        public string Pay_Period { get; set; } = "";

    }
}
