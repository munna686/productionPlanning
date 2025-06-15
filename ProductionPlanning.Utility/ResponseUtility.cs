using ProductionPlanning.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Utility
{
    public static class ResponseUtility
    {
        public static string LoginSuccessMessage = "Login successful";
        public static string InvalidPasswordMessage = "Email or Password Invalid";
        public static string SuccessMessage = "Data Saved Successfully";
        public static string UpdateMessage = "Data Updated Successfully";
        public static string HardDeleteMessage = "Data fully deleted Successfully";
        public static string SoftDeleteMessage = "Data deleted Successfully";
        public static string GetAllMessage = "The data has been successfully fetched";
        public static string FailMessage = "Data Saved Failed";
        public static string SuccessPageOperationAccessMessage = "User is authorized to access this operation";
        public static string FailPageOperationAccessMessage = "User is not authorized to access this operation";

        public static ServiceResponse SendSuccessResponce(this object responseData)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            serviceResponse.Data = responseData;
            serviceResponse.Message = SuccessMessage;
            serviceResponse.Success = true;

            return serviceResponse;
        }

        public static AuthResponseDTO SendLoginFail(LoginDTO dto)
        {
            return new AuthResponseDTO
            {
                AccessToken = null,
                RefreshToken = null,
                UserName = dto.Email, // অথবা null
            };
        }

        public static ServiceResponse SendUpdateResponce(this object responseData)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            serviceResponse.Data = responseData;
            serviceResponse.Message = UpdateMessage;
            serviceResponse.Success = true;
            return serviceResponse;
        }
        public static ServiceResponse SendSoftDeleteResponce(this object responseData)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            serviceResponse.Data = responseData;
            serviceResponse.Message = SoftDeleteMessage;
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public static ServiceResponse SendGetAllResponce(this object responseData)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            serviceResponse.Data = responseData;
            serviceResponse.Message = GetAllMessage;
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public static ServiceResponse SendHardDeleteResponce(this object responseData)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            serviceResponse.Data = responseData;
            serviceResponse.Message = HardDeleteMessage;
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public static ServiceResponse SendFailResponce(this object responseData)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            serviceResponse.Data = responseData;
            serviceResponse.Message = FailMessage;
            serviceResponse.Success = false;
            return serviceResponse;
        }

        public static ServiceResponse SendSuccessPageOperationAccessResponce(this object responseData)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            serviceResponse.Data = responseData;
            serviceResponse.Message = SuccessPageOperationAccessMessage;
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public static ServiceResponse SendFailPageOperationAccessResponce(this object responseData)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            serviceResponse.Data = responseData;
            serviceResponse.Message = FailPageOperationAccessMessage;
            serviceResponse.Success = false;
            return serviceResponse;

        }
        public static ServiceResponse SendLoginFail(this object responseData)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            serviceResponse.Data = responseData;
            serviceResponse.Message = InvalidPasswordMessage;
            serviceResponse.Success = false;
            return serviceResponse;
        }
        public static ServiceResponse SendLoginSuccess(this object responseData)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            serviceResponse.Data = responseData;
            serviceResponse.Message = LoginSuccessMessage;
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public static ServiceResponse SendLogOutSuccess(this object responseData)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            serviceResponse.Data = "Sucess";
            serviceResponse.Message = "Logout Success";
            serviceResponse.Success = true;
            return serviceResponse;
        }
    }
}
