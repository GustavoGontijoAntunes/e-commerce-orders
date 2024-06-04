using AutoMapper;
using eCommerceOrders.FinishPurchase.Dtos;
using eCommerceOrders.FinishPurchase.Models;
using eCommerceOrders.FinishPurchase.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceOrders.FinishPurchase.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductResult>>> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            var productsResult = _mapper.Map<List<ProductResult>>(products);

            return Ok(productsResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResult>> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            var productResult = _mapper.Map<ProductResult>(product);

            if (productResult == null)
            {
                return NotFound();
            }

            return Ok(productResult);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductPost productPost)
        {
            var product = _mapper.Map<Product>(productPost);
            var createdProduct = await _productService.CreateProductAsync(product);

            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductPost productPost)
        {
            if (id != productPost.Id)
            {
                return BadRequest();
            }

            var product = _mapper.Map<Product>(productPost);
            var updatedProduct = await _productService.UpdateProductAsync(product);

            if (updatedProduct == null)
            {
                return NotFound();
            }

            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _productService.DeleteProductAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}