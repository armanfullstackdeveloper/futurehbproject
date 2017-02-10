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
        InvalidNumber = 5,
        SystemISDisable = 6,
        BadWords = 7,
        PardisMinimumReceivers = 8,
        NumberIsPublic = 9,
        UnKnown=10,//was not in doc
        Failed=11
    }
}
