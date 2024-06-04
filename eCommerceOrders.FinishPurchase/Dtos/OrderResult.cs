namespace eCommerceOrders.FinishPurchase.Dtos
{
    public class OrderResult
    {
        public int Id { get; set; }
        public bool IsFinished { get; set; }
        public decimal TotalValue { get; set; }

        public List<OrderProductResult> OrderProducts { get; set; }
    }
}