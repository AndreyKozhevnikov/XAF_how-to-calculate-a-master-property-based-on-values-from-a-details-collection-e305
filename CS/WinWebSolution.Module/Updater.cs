using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

namespace WinWebSolution.Module {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
                Product product = ObjectSpace.CreateObject<Product>();
                product.Name = "Chai";
                for (int i = 0; i < 10; i++) {
                    Order order = ObjectSpace.CreateObject<Order>();
                    order.Product = product;
                    order.Description = "Order " + i.ToString();
                    order.Total = i;
                }
                ObjectSpace.CommitChanges();
            
        }
    }
}