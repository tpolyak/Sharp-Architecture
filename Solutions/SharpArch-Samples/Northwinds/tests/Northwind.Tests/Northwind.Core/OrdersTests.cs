using System;
using System.Collections.Generic;
using NUnit.Framework;
using Northwind.Core;
using System.Diagnostics;
using SharpArch.Testing.NUnit;

namespace Tests.Northwind.Core
{
    [TestFixture]
    public class OrdersTests
    {
        [Test]
        public void CanFindOrdersByDate() {
            IList<Order> orders = GetExampleOrders();
            List<Order> ordersPlacedOnDate = orders.FindOrdersPlacedOn(new DateTime(2008, 3, 20));

            foreach (Order order in ordersPlacedOnDate) {
                Debug.WriteLine(order.OrderDate.ToString());
            }

            ordersPlacedOnDate.ShouldNotBeNull();
            ordersPlacedOnDate.Count.ShouldEqual(2);
        }

        public List<Order> GetExampleOrders() {
            List<Order> orders = new List<Order>();
            orders.Add(CreateExampleOrderWith(new DateTime(2008, 3, 20)));
            orders.Add(CreateExampleOrderWith(new DateTime(2008, 3, 20)));
            orders.Add(CreateExampleOrderWith(new DateTime(2008, 3, 19)));

            return orders;
        }

        public Order CreateExampleOrderWith(DateTime orderedDate) {
            Customer orderedBy = new Customer("Acme Anvil");
            Order order = new Order(orderedBy);
            order.OrderDate = orderedDate;

            return order;
        }
    }
}
