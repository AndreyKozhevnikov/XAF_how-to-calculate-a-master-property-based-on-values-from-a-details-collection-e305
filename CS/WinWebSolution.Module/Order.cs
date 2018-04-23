using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;

namespace WinWebSolution.Module {
    public class Order : BaseObject {
        public Order(Session session) : base(session) { }
        private string fDescription;
        public string Description {
            get { return fDescription; }
            set { SetPropertyValue("Description", ref fDescription, value); }
        }
        private decimal fTotal;
        public decimal Total {
            get { return fTotal; }
            set {
                SetPropertyValue("Total", ref fTotal, value);
                if(!IsLoading && !IsSaving && Product != null) {
                    Product.UpdateOrdersTotal(true);
                    Product.UpdateMaximumOrder(true);
                }
            }
        }
        private Product fProduct;
        [Association("Product-Orders")]
        public Product Product {
            get { return fProduct; }
            set {
                Product oldProduct = fProduct;
                SetPropertyValue("Product", ref fProduct, value);
                if (!IsLoading && !IsSaving && oldProduct != fProduct) {
                    oldProduct = oldProduct ?? fProduct;
                    oldProduct.UpdateOrdersCount(true);
                    oldProduct.UpdateOrdersTotal(true);
                    oldProduct.UpdateMaximumOrder(true);
                }
            }
        }
    }
}