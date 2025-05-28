
using Dapper;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Models;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;


namespace QPay.BAL.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly DbRepository _dbRepository;      


        public LoginRepository(DbRepository dbRepository)
        {
            this._dbRepository=dbRepository;
        }
        public Users UserLogin(int userName, string password, string loginIp, string CName)
        {

            Users userDetails = null;
            
            var newPasswordSalt = GetSalt(userName);
            var salt = PasswordHashAlgorithm.GetSaltBase64Decoded(newPasswordSalt.Salt);
            var  encodedPassword = password != null ? PasswordHashAlgorithm.GetHashBase64EncodedPassword(password, salt) : string.Empty;
            string storeProcedure = string.Format("sp_IsValidUser1");
            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userName);
            parameters.Add("@Password", encodedPassword);
            parameters.Add("@LoginIP", loginIp);
            parameters.Add("@ComputerName", CName);
          
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if(res!="")
            {
                userDetails = JsonConvert.DeserializeObject<List<Users>>(res).FirstOrDefault();
                return userDetails;
            }
            return userDetails;
        }

        public bool Ismatching(int input,int output)
        {
            if(input==output)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Payload GetCompanies()
        {
            Payload payload = new Payload();

            List<CompanyData> companies = new List<CompanyData> 
            { 
                new CompanyData {  
                CompanyCode="PSL0003253",
                Category="P4", 
                AssignmentNumber=101, 
                HeadCount="1345 HC",
                Revised="1 Revised", 
                EstimateTime="00:20:00",
            NewJoinee=new NewJoinee{Input=50,Output=50,Ismatching =Ismatching(50,50),Remarks="" },
            Attendance=new Attendance{Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
            Adhoc=new Adhoc {Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
            Increment =new Increment{Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
            OtherInput=new  OtherInput {Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"}},

            new CompanyData { 
                CompanyCode="PSL0003254", 
                Category="P1", 
                AssignmentNumber=106, 
                HeadCount="540 HC", 
                Revised="0 Revised", 
                EstimateTime="00:10:00",
            NewJoinee=new NewJoinee{Input=50,Output=50,Ismatching =Ismatching(50,50),Remarks="" },
            Attendance=new Attendance{Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
            Adhoc=new Adhoc {Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
            Increment =new Increment{Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
            OtherInput=new  OtherInput {Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"}},

            new CompanyData { 
                CompanyCode="PSL0003255", 
                Category="P3",
                AssignmentNumber=107, 
                HeadCount="986 HC", 
                Revised="3 Revised", 
                EstimateTime="00:15:00",NewJoinee=new NewJoinee{Input=50,Output=50,Ismatching =Ismatching(50,50),Remarks="" },
            Attendance=new Attendance{Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
            Adhoc=new Adhoc {Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
            Increment =new Increment{Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
            OtherInput=new  OtherInput {Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"} },

            new CompanyData { CompanyCode="PSL0003256",
                Category="P3", 
                AssignmentNumber=108, 
                HeadCount="10 HC", 
                Revised="1 Revised", 
                EstimateTime="00:08:00",
            Attendance=new Attendance{Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
                Adhoc=new Adhoc {Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
                Increment =new Increment{Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
                OtherInput=new  OtherInput {Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"}},

            new CompanyData { CompanyCode="PSL0003257", 
                Category="P2", AssignmentNumber=109, 
                HeadCount="102 HC", 
                Revised="0 Revised", 
                EstimateTime="00:05:00",NewJoinee=new NewJoinee{Input=50,Output=50,Ismatching =Ismatching(50,50),Remarks="" },
                Attendance=new Attendance{Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
                Adhoc=new Adhoc {Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
                Increment =new Increment{Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
                OtherInput=new  OtherInput {Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"} }};
            
        payload.Yesterday_Lot=companies;

            List<CompanyData> companies_T = new List<CompanyData>
            { new CompanyData { CompanyCode="PSL0003255",
                Category="P2",
                AssignmentNumber=201,
                HeadCount="125 HC",
                Revised="4 Revised",
                EstimateTime="00:10:00" ,
                NewJoinee=new NewJoinee{Input=50,Output=50,Ismatching =Ismatching(50,50),Remarks="" },
                Attendance=new Attendance{Input=20,Output=20,Ismatching=Ismatching(20,20),Remarks="Variance"},
                Adhoc=new Adhoc {Input=25,Output=25,Ismatching=Ismatching(25,25),Remarks="Variance"},
                Increment =new Increment{Input=37,Output=37,Ismatching=Ismatching(37,37),Remarks="Variance"},
                OtherInput=new  OtherInput {Input=29,Output=29,Ismatching=Ismatching(29,29),Remarks="Variance"}},

            new CompanyData { CompanyCode="PSL0003260",
                Category="P3",
                AssignmentNumber=202,
                HeadCount="999 HC",
                Revised="0 Revised",
                EstimateTime="00:10:00",
                NewJoinee=new NewJoinee{Input=50,Output=50,Ismatching =Ismatching(50,50),Remarks="" },
                Attendance=new Attendance{Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
                Adhoc=new Adhoc {Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
                Increment =new Increment{Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
                OtherInput=new  OtherInput {Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"}},

            new CompanyData { CompanyCode="PSL0003262",
                Category="P4",
                AssignmentNumber=204,
                HeadCount="986 HC",
                Revised="0 Revised",
                EstimateTime="00:15:00",
                NewJoinee=new NewJoinee{Input=50,Output=50,Ismatching =Ismatching(50,50),Remarks="" },
                Attendance=new Attendance{Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
                Adhoc=new Adhoc {Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
                Increment =new Increment{Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"},
                OtherInput=new  OtherInput {Input=50,Output=48,Ismatching=Ismatching(50,48),Remarks="Variance"}}};

            payload.TodayDay_Lot=companies_T;
            return payload;
        }

        public PasswordSalt GetSalt(int employeeCode)
        {
            string storeProcedure = string.Format("spGetSalt");
            PasswordSalt salt = new PasswordSalt();
            var parameters = new DynamicParameters();
            parameters.Add("@Employee_code", employeeCode);
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res!="")
            {
                 salt = JsonConvert.DeserializeObject<PasswordSalt[]>(res).FirstOrDefault();
                return salt;
            }
            return salt;
        }
    }

    internal class PasswordHashAlgorithm
    {
        public static byte[] GenerateSalt()
        {
            byte[] salt = Guid.NewGuid().ToByteArray();
            return salt;
        }
        public static byte[] GetSalt(string encodedSalt)
        {

            if (!string.IsNullOrEmpty(encodedSalt))
                return GetSaltBase64Decoded(encodedSalt);
            else
                return null;
        }
        //public static byte[] GetSalt(string encodedSalt)
        //{
        //    string encodedSalt = null;

        //    using (ModelDBContext context = Utility.GetDBContext())
        //    {
        //        context.Database.Connection.Open();
        //        DbCommand cmd = context.Database.Connection.CreateCommand();
        //        cmd.CommandText = "spGetSalt";
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add(new SqlParameter("@Employee_code", employee_code));
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                encodedSalt = Convert.ToString(reader["Salt"]);
        //            }
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(encodedSalt))
        //        return GetSaltBase64Decoded(encodedSalt);
        //    else
        //        return null;
        //}
        public static string GetSaltBase64Encoded(byte[] salt)
        {
            return Convert.ToBase64String(salt);
        }
        public static byte[] GetSaltBase64Decoded(string salt)
        {
            return Convert.FromBase64String(salt);
        }
        public static string GetHashBase64EncodedPassword(string plainText,
                                 byte[] saltBytes)
        {

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
                    new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];


            HashAlgorithm hash;

            hash = new SHA256Managed();

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }
    }
}
