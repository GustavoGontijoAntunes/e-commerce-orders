namespace eCommerceOrders.FinishPurchase.Dtos
{
    public class OrderPost
    {
        public int Id { get; set; }
        public bool IsFinished { get; set; }
        public List<OrderProductPost> Products { get; set; }
    }
}