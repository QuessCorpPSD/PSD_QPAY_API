using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.GlobalMaster
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "ClientDetails")]
    [System.Serializable()]
    public class ClientResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("Client")]
        public Client[] ClientDetails { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_Client")]
    public class Client
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Client_Id { get; set; }

        public string Client_Code { get; set; }
        public string Client_Name { get; set; }
        //public string Function_Code { get; set; }
       // public int Serial_No { get; set; }
      //  public string Error_Message { get; set; }
    }

    public class ClientRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public Client detail { get; set; }
    }
}