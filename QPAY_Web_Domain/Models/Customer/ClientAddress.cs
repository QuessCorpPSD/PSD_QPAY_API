using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QPay.UI.Models.Customer
{
    public class ClientAddress
    {
        public int? ClientAddressId { get; set; }
        public int? CompanyId { get; set; }
        public string? Company_Code { get; set; }
        public int? StateId { get; set; }
        public string? State_Name { get; set; }
        public int? CostCenterMappingId { get; set; }
        public string? Map_Name { get; set; }
        public string? BillingClientName { get; set; }
        public string? BillingAddress { get; set; }
        public int? BillingStateId { get; set; }
        public string? BillingStateName { get; set; }
        public Boolean IsShippingAddressSameAsBilling { get; set; }
        public string? ShippingClientName { get; set; }
        public string? ShippingAddress { get; set; }
        public int? ShippingStateId { get; set; }
        public string? ShippingStateName { get; set; }
        public string EffectiveDate { get; set; }
        public Boolean SEZ_Applicable { get; set; }
        public byte[] SEZ_Document { get; set; }
        public DateTime? SEZ_ExpiryDate { get; set; }
        public string? LUT_Number { get; set; }
        public DateTime? LUT_Date { get; set; }
        public DateTime? LUT_ExpiryDate { get; set; }
        public string? VendorCode { get; set; }
        public string? SAC_Code { get; set; }
        public Boolean GstApplicable { get; set; }
        public string? GstNumber { get; set; }
        public int CreatedBy { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? City_Name { get; set; }
        public string? ShippingCity_Name { get; set; }
        public string? ShippingPinCode { get; set; }
        public string? BillingPinCode { get; set; }
        public string? SapBillTo { get; set; }
        public string? SapShipTo { get; set; }
        public string? AddressCode { get; set; }
    }

    public class AddressRequest
    {
        public string? Action { get; set; }
        public string? SearchText { get; set; }
        public int? UserId { get; set; }
        public string? XmlData { get; set; }
        public int? ClientAddressId { get; set; }
        public int? CompanyId { get; set; }
        public int? StateId { get; set; }
        public int? CostCenterMappingId { get; set; }
        public string? BillingClientName { get; set; }
        public string? BillingAddress { get; set; }
        public int? BillingStateId { get; set; }
        public Boolean IsShippingAddressSameAsBilling { get; set; }
        public string? ShippingClientName { get; set; }
        public string? ShippingAddress { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? SAC_Code { get; set; }
        public string? GstNumber { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
        public string? SortField { get; set; }
        public string? SortDirection { get; set; }
        public int? TotalCount { get; set; }
        public string? Company_Code { get; set; }
        public string? State_Name { get; set; }
        public string? Map_Name { get; set; }
        public string? BillingStateName { get; set; }
        public string? ShippingStateName { get; set; }
        public string? ClientGstNumber { get; set; }
        public Boolean GstApplicable { get; set; }
    }

    public class ClientAddressResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }

}
