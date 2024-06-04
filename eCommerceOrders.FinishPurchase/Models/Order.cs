namespace eCommerceOrders.FinishPurchase.Models
{
    public class Order
    {
        public Order() { }

        public int Id { get; set; }
        public bool IsFinished { get; set; }
        public decimal TotalValue { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }
}