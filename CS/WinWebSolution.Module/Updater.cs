using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Updating;

namespace WinWebSolution.Module {
    public class Updater : ModuleUpdater {
        public Updater(Session session, Version currentDBVersion) : base(session, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            using (UnitOfWork uow = new UnitOfWork(Session.DataLayer)) {
                Product product = new Product(uow);
                product.Name = "Chai";
                for (int i = 0; i < 10; i++) {
                    Order order = new Order(uow);
                    order.Product = product;
                    order.Description = "Order " + i.ToString();
                    order.Total = i;
                }
                uow.CommitChanges();
            }
        }
    }
}