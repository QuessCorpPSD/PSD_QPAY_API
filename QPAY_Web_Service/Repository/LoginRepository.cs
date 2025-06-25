
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Admin;
using QPay.UI.Common;
using QPay.UI.Models;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace QPay.BAL.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly DbRepository _dbRepository;      


        public LoginRepository(DbRepository dbRepository)
        {
            this._dbRepository=dbRepository;
        }
      
        public async Task<List<QPay.UI.Models.Users>> GetAllActiveUsers()
        {
            List<QPay.UI.Models.Users> users = new List<QPay.UI.Models.Users>();

            try
            {
                string sql = string.Format("select u.*,ur.RoleId Role_Id from tbl_user u left outer join userrole ur on u.User_Id = ur.UserId Where  ur.UserId is not null and u.IsActive ={0}", 1);
                var status = await this._dbRepository.QueryMultiAsync(sql);
                users = JsonConvert.DeserializeObject<List<QPay.UI.Models.Users>>(status)
                                                   ?? new List<QPay.UI.Models.Users>();
            }
            catch (JsonException ex)
            {
                // Log the error if needed
                users = new List<QPay.UI.Models.Users>();
            }

            return users;

        }
        public async Task<QPay.UI.Models.Users?> UserLogin(int userName, string password, string loginIp, string computerName)
        {
            QPay.UI.Models.Users users = new UI.Models.Users();
            try
            {
                // Get salt and hash the password
                var newPasswordSalt = GetSalt(userName);
                var salt = PasswordHashAlgorithm.GetSaltBase64Decoded(newPasswordSalt.Salt);
                var encodedPassword = string.IsNullOrWhiteSpace(password)
                    ? string.Empty
                    : PasswordHashAlgorithm.GetHashBase64EncodedPassword(password, salt);

                // Prepare parameters
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", userName);
                parameters.Add("@Password", encodedPassword);
                parameters.Add("@LoginIP", loginIp);
                parameters.Add("@ComputerName", computerName);

                // Call stored procedure
                var resultJson = await _dbRepository.GetItemsAsync("sp_IsValidUser1", parameters);

                // Deserialize and return
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    if (JsonConvert.DeserializeObject<List<QPay.UI.Models.Users>>(resultJson) is { Count: > 0 } userList &&
                    userList.FirstOrDefault() is { } user)
                    {
                        users = user;
                        return users;
                    }
                    else
                    {
                        users = new UI.Models.Users();
                         users.Error_Message = "Invalid UserName and Password";
                        return users;
                    }
                }
                else
                {
                    users = new UI.Models.Users();
                    users.Error_Message = "Invalid UserName and Password";
                    return users;
                }

                
            }
            catch (Exception ex)
            {
                // You can log the exception here using ILogger
                // Console.WriteLine($"Login failed: {ex.Message}");
                users.Error_Message= ex.Message;
                return users;
            }
        }

        public async Task<QPay.UI.Models.Users?> UserCreate(QPay.UI.Models.Users user)
        {
            try
            {
                var rowsalt = (byte[])PasswordHashAlgorithm.GenerateSalt();
                var salt = PasswordHashAlgorithm.GetSaltBase64Encoded(rowsalt);
                var password = PasswordHashAlgorithm.GetHashBase64EncodedPassword(user.Password, rowsalt);

                UserDetails userDetails = new UserDetails()
                {
                    User_Id=0,
                    Name=user.Name,
                    
                    Password=password,
                    Salt=salt,
                    Mail_Id=user.Mail_Id,
                    Reporting_To=user.Reporting_To,
                    Role_Id=user.Role_Id,
                    Access_Type_Id=user.Access_Type_Id,
                    EmployeeID=user.EmployeeID,
                    IsActive=user.IsActive
                };

                var userResponse = new UserResponse();
                userResponse.UserDetails = new UserDetails[1];
                userResponse.UserDetails[0] = userDetails;
                string userResponseSerialize = "";
                userResponseSerialize = GenericSerializer<UserResponse>.Serialize(userResponse);
                var parameters = new DynamicParameters();
                parameters.Add("@xmlInput", userResponseSerialize);
                parameters.Add("@mode", "Add");
                parameters.Add("@CreatedBy", user.CreatedBy);
                parameters.Add("@Process_Category", user.Process_Category);
                parameters.Add("@TeamLead_User_Id", user.TeamLeadUserId);
                parameters.Add("@TeamLead_Email_Id", user.TeamLeadEmailId);
                parameters.Add("@Manager_User_Id", user.Manager_User_Id);
                parameters.Add("@Manager_Email_Id", user.Manager_Email_Id);
                parameters.Add("@Fun_Head_UserId", user.Fun_Manager_User_Id);
                parameters.Add("@Fun_Head_EmailId", user.Fun_Manager_Email_Id);
                // Call stored procedure
                var resultJson = await _dbRepository.GetItemsAsync("sp_CreateUpdateUserDetails", parameters);

                // Deserialize and return
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    var users = JsonConvert.DeserializeObject<List<QPay.UI.Models.Users>>(resultJson);
                    return users?.FirstOrDefault();
                }

                return null;
            }
            catch (Exception ex)
            {
                // You can log the exception here using ILogger
                Console.WriteLine($"Login failed: {ex.Message}");
                return null;
            }
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

        public async Task<QPay.UI.Models.Users?> ChangePasswordAsync(ChangePassword changePassword)
        {
            if (changePassword == null)
                throw new ArgumentNullException(nameof(changePassword));

            //byte[] salt = PasswordHashAlgorithm.GetSalt(changePassword.User_Id.ToString()) ?? PasswordHashAlgorithm.GenerateSalt();
            var salt = GetSalt(changePassword.employeeId);
            var salts = PasswordHashAlgorithm.GetSaltBase64Decoded(salt.Salt);
          var  oldpassword = string.IsNullOrWhiteSpace(changePassword.OldPassword)
                ? string.Empty
                : PasswordHashAlgorithm.GetHashBase64EncodedPassword(changePassword.OldPassword, salts);
            var currnt = PasswordHashAlgorithm.GetSaltBase64Decoded(salt.Salt);
            var password = new ChangePassword
            {
                User_Id= changePassword.User_Id,
                UserName= changePassword.UserName,
                OldPassword = oldpassword,
                NewPassword = PasswordHashAlgorithm.GetHashBase64EncodedPassword(changePassword.NewPassword ?? string.Empty, currnt),
                ConformNewPassword = PasswordHashAlgorithm.GetHashBase64EncodedPassword(changePassword.ConformNewPassword ?? string.Empty, currnt),
                Salt = PasswordHashAlgorithm.GetSaltBase64Encoded(currnt)
            };

            var changePwdResponse = new ChangePwdResponse
            {
                ChangePasswordUser = [password] 
            };

            string serializedInput = GenericSerializer<ChangePwdResponse>.Serialize(changePwdResponse);

            var parameters = new DynamicParameters();
            parameters.Add("@xmlInput", serializedInput);

            var resultXml = await _dbRepository.GetItemsAsync("sp_UpdateChangePassword", parameters);
            if (string.IsNullOrWhiteSpace(resultXml))
                return null;

            var users = JsonConvert.DeserializeObject<List<QPay.UI.Models.Users>>(resultXml);
            return users?.FirstOrDefault();
        }
        public  string GetSaltByEmployeeId(int encodedSalt,string password)
        {
            string ret_password = string.Empty;

            var salt = GetSalt(encodedSalt);
            var salts = PasswordHashAlgorithm.GetSaltBase64Decoded(salt.Salt);
            ret_password = string.IsNullOrWhiteSpace(password)
                ? string.Empty
                : PasswordHashAlgorithm.GetHashBase64EncodedPassword(password, salts);
            return ret_password;

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
