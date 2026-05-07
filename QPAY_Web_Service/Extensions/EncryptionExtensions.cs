using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Extensions
{
    public static class EncryptionExtensions
    {
        public static string ToEncrypt(this string stringToEncrypt)
        {
            try
            {
                byte[] encode = Encoding.UTF8.GetBytes(stringToEncrypt);
                return Convert.ToBase64String(encode);
            }
            catch (Exception ex)
            {
                throw new Exception("Error during encoding", ex);
            }
        }
    }
}
