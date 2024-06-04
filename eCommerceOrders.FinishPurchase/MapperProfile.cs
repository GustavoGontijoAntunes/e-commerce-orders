using AutoMapper;
using eCommerceOrders.FinishPurchase.Dtos;
using eCommerceOrders.FinishPurchase.Models;
using eCommerceOrders.FinishPurchase.Services.Interfaces;

namespace eCommerceOrders.FinishPurchase
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            #region Product
            CreateMap<ProductPost, Product>();
            CreateMap<Product, ProductResult>();
            #endregion

            #region Order
            CreateMap<OrderPost, Order>()
                .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom<OrderProductResolver>());
            CreateMap<Order, OrderResult>();
            #endregion

            #region OrderProduct
            CreateMap<Order, OrderResult>()
                .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.OrderProducts));
            CreateMap<OrderProduct, OrderProductResult>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
            #endregion
        }
    }
}

public class OrderProductResolver : IValueResolver<OrderPost, Order, List<OrderProduct>>
{
    private readonly IProductService _productService;

    public OrderProductResolver(IProductService productService)
    {
        _productService = productService;
    }

    public List<OrderProduct> Resolve(OrderPost source, Order destination, List<OrderProduct> destMember, ResolutionContext context)
    {
        var products = _productService.GetProductsByIdsAsync(source.Products.Select(p => p.ProductId).ToList()).Result;

        var orderProducts = source.Products
            .Select(opDto => new OrderProduct
            {
                ProductId = opDto.ProductId,
                Quantity = opDto.Quantity,
                Product = products.First(p => p.Id == opDto.ProductId)
            })
            .ToList();

        return orderProducts;
    }
}