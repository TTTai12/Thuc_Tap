using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TT_ECommerce.Data;
using TT_ECommerce.Models.EF;

namespace TT_ECommerce.Components
{
    public class ProductSaleViewComponent : ViewComponent
    {
        private readonly TT_ECommerceDbContext _context;

        public ProductSaleViewComponent(TT_ECommerceDbContext context)
        {
            _context = context;
        }

        // Phương thức chính của View Component
        public IViewComponentResult Invoke(int topCount = 10)
        {
            // Lấy danh sách sản phẩm có giá rẻ nhất (sắp xếp trên bộ nhớ để tránh hạn chế ORDER BY decimal của SQLite)
            var cheapProducts = _context.TbProducts
                .AsEnumerable()
                .OrderBy(p => p.PriceSale > 0 ? p.PriceSale : p.Price)
                .Take(topCount)
                .ToList();

            return View(cheapProducts);
        }
    }
}
