using Boundary.Helper.StaticValue;
using DataModel.Models.ViewModel;

namespace Boundary.Helper
{
    public class JsonResultHelper
    {
        public static JsonResultViewModel SuccessResult(dynamic response=null)
        {
            return new JsonResultViewModel()
            {
                Response = response??StaticString.Message_SuccessFull,
                Success = true,
            };
        }

        public static JsonResultViewModel FailedResultWithTrackingCode(long trackingCode)
        {
            return new JsonResultViewModel()
            {
                Response = StaticString.Message_UnSuccessFull,
                Success = false,
                TrackingCode = trackingCode.ToString()
            };
        }

        public static JsonResultViewModel FailedResultWithMessage(string response=null)
        {
            return new JsonResultViewModel()
            {
                Response = response ?? StaticString.Message_UnSuccessFull,
                Success = false,
            };
        }

        public static JsonResultViewModel FailedResultOfWrongAccess()
        {
            return new JsonResultViewModel()
            {
                Response = StaticString.Message_WrongAccess,
                Success = false,
            };
        }

        public static JsonResultViewModel FailedResultOfInvalidInputs()  
        {
            return new JsonResultViewModel()
            {
                Response = StaticString.Message_InvalidInputs,
                Success = false,
            };
        }

    }
}