namespace eCommerceOrders.FinishPurchase.Services.Interfaces
{
    public interface IBusService
    {
        void Publish<T>(T message);
    }
}