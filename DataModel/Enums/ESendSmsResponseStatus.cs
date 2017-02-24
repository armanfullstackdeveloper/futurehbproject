using System.ComponentModel;

namespace DataModel.Enums
{
    public enum ESendSmsResponseStatus : byte
    {
        InvalidUserPass = 0,
        Successfull = 1,
        NoCredit = 2,
        DailyLimit = 3,
        SendLimit = 4,
        InvalidSenderNumber = 5,
        SystemISDisable = 6,
        BadWords = 7,
        PardisMinimumReceivers = 8,
        NumberIsPublic = 9,
        InactiveUser=10,
        Failed=11,
        UserInfoNotRegister=12
    }
}
