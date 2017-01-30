using System.Collections.Generic;
using DataModel.Entities.RelatedToOrder;
using DataModel.Enums;

namespace DataModel.Models.ViewModel
{
    public class OrderViewModel
    {        
        #region نمایش اولیه

        public long OrderCode { get; set; }
        public int Date { get; set; }
        public int Time { get; set; }        

        #endregion



        #region نمایش جزئیات

        public int PostalCost { get; set; }
        public string OrderSendingTypeName { get; set; }
        public string TrackingCode { get; set; }
        public List<OrderProductsViewModel> ProductDetailes { get; set; }

        #endregion

    }

    public class OrderViewModelForMembers : OrderViewModel
    {
        public string StatusName { get; set; } //وضعیت 
        public string OrderTypeName { get; set; } //شیوه پرداخت
        public string ShopName { get; set; }
        public int OverallPayment { get; set; } //مبلغ کل پرداختی
        public int OverallDiscount { get; set; } //مجموع تخفیف
        public List<DropDownItemsModel> EditableStatus { get; set; } 
    }

    public class OrderViewModelForStores : OrderViewModel
    {
        public string StatusName { get; set; } //وضعیت 
        public string OrderTypeName { get; set; } //شیوه پرداخت
        public string MemberName { get; set; }
        public int OverallIncome { get; set; } //مجموع درامد
        public List<DropDownItemsModel> EditableStatus { get; set; } 
    }

    public class OrderViewModelForAdmins : OrderViewModel
    {
        #region نمایش اولیه

        public EOrderType OrderType { get; set; }
        public OrderStatus OrderStatus { get; set; }  
        public string ShopName { get; set; }
        public string MemberName { get; set; }
        public int OverallPayment { get; set; } //مبلغ پرداختی کاربر
        public int MemberUsedBalance { get; set; } //مبزان استفاده از موجودی قبلی کاربر     
        public int OverallOrderCost { get; set; } //مجموع هزینه ای که این سفارش داشته
        public bool IsPony { get; set; } //تسویه حساب با فروشنده
        public bool CanPony { get; set; } //هم اکنون قادر به تسویه حساب هستیم یا نه

        #endregion



        #region نمایش جزئیات

        public int OverallProductCostWithoutConsideringDiscount { get; set; }
        public int OverallProductCostWithConsideringDiscount { get; set; }
        public int OverallOrderCostWithoutConsideringDiscount { get; set; }
        public int OverallOrderCostWithConsideringDiscount { get; set; }
        public StoreDiscount StoreDiscount { get; set; }
        public int DiscountOfStoreDiscountCode { get { return OverallDiscount - MemberUsedBalance; } }
        public int DiscountOfMemberUsedBalance  { get { return MemberUsedBalance; } }
        public int OverallDiscount { get; set; } // مجموع تخفیف که هم شامل کد تخفیف و هم موجودی قبلی میشه
        public List<OrderHistoryViewModel> OrderHistories { get; set; } 

        #endregion
    }
}
