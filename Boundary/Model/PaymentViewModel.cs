using System.Collections.Generic;
using DataModel.Enums;
using DataModel.Models.ViewModel;

namespace Boundary.Model
{
    public class PaymentViewModel
    {
        private EOrderType? _orderType;
        private List<ShoppingBagViewModel> _bag;
        private EPaymentGateway? _paymentGateway;

        public EOrderType OrderType
        {
            get { return _orderType ?? EOrderType.SecurePayment; }
            set { _orderType = value; }
        }

        public List<ShoppingBagViewModel> Bag
        {
            get { return _bag; }
            set { _bag = value; }
        }

        public EPaymentGateway PaymentGateway
        {
            get { return _paymentGateway ?? EPaymentGateway.Pasargad; }
            set { _paymentGateway = value; }
        }

        public string DiscountCode { get; set; }
    }
}