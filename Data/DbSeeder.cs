using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TT_ECommerce.Models.EF;

namespace TT_ECommerce.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            TT_ECommerceDbContext db,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Nếu đã có dữ liệu rồi thì không seed nữa
            var hasData =
                await db.TbProducts.AnyAsync() ||
                await db.TbOrders.AnyAsync() ||
                userManager.Users.Any();

            if (hasData)
            {
                return;
            }

            var now = DateTime.UtcNow;
            var rnd = new Random();

            // Danh sách ảnh có thật trong wwwroot (đã có sẵn trên GitHub)
            var productImagePool = new[]
            {
                "/imgProducts/1222.jpg",
                "/imgProducts/LG.png",
                "/imgProducts/acer.png",
                "/imgProducts/asus.png",
                "/imgProducts/msi.png",
                "/imgProducts/legion5.png",
                "/imgProducts/heliosneo14.png",
                "/imgProducts/ip16.png",
                "/imgProducts/ip16pro.png",
                "/imgProducts/ip15pro.png",
                "/imgProducts/tuf.png",
                "/imgProducts/victus.png",
                "/imgProducts/samsung-zfold.png",
                "/imgProducts/zflip.png",
                "/imgProducts/nitrov15.png"
            };

            var postImagePool = new[]
            {
                "/imgPosts/posts1.jpg",
                "/imgPosts/posts2.jpg",
                "/imgPosts/posts3.jpg",
                "/imgPosts/posts4.jpg",
                "/imgPosts/posts5.jpg",
                "/imgPosts/post8.jpg",
                "/imgPosts/amd.jpg",
                "/imgPosts/1.jpg",
                "/imgPosts/avt.jpg"
            };

            // 1. Tạo role
            var roles = new[] { "ADMIN", "USER" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Tạo user
            var users = new List<IdentityUser>();

            var adminUser = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                EmailConfirmed = true
            };

            if (await userManager.FindByNameAsync(adminUser.UserName) == null)
            {
                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "ADMIN");
                users.Add(adminUser);
            }

            for (int i = 1; i <= 100; i++)
            {
                var userName = $"user{i}";
                if (await userManager.FindByNameAsync(userName) != null)
                {
                    continue;
                }

                var user = new IdentityUser
                {
                    UserName = userName,
                    Email = $"user{i}@example.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, "User@123");
                await userManager.AddToRoleAsync(user, "USER");
                users.Add(user);
            }

            // 3. Tạo categories
            var categories = new List<TbProductCategory>();
            for (int i = 1; i <= 5; i++)
            {
                categories.Add(new TbProductCategory
                {
                    Title = $"Category {i}",
                    Description = $"Demo category {i}",
                    Icon = null,
                    SeoTitle = $"Category {i}",
                    SeoDescription = $"Demo category {i}",
                    SeoKeywords = $"category{i}",
                    CreatedBy = "seed",
                    CreatedDate = now,
                    ModifiedDate = now,
                    Modifiedby = "seed",
                    Alias = $"category-{i}"
                });
            }

            db.TbProductCategories.AddRange(categories);
            await db.SaveChangesAsync();

            // 4. Tạo sản phẩm (nhiều dữ liệu hơn)
            var products = new List<TbProduct>();
            for (int i = 1; i <= 300; i++)
            {
                var category = categories[rnd.Next(categories.Count)];
                var basePrice = rnd.Next(100, 1000); // 100 - 1000
                var hasSale = i % 3 == 0;

                products.Add(new TbProduct
                {
                    Title = $"Demo Product {i}",
                    ProductCode = $"P{i:0000}",
                    Description = $"Demo product {i}",
                    Detail = $"This is demo product {i}.",
                    Image = productImagePool[rnd.Next(productImagePool.Length)],
                    Price = basePrice,
                    PriceSale = hasSale ? basePrice * 0.9m : null,
                    Quantity = rnd.Next(1, 100),
                    IsHome = true,
                    IsSale = hasSale,
                    IsFeature = i % 5 == 0,
                    IsHot = i % 7 == 0,
                    ProductCategoryId = category.Id,
                    SeoTitle = $"Demo Product {i}",
                    SeoDescription = $"Demo product {i}",
                    SeoKeywords = $"product{i}",
                    CreatedBy = "seed",
                    CreatedDate = now.AddDays(-rnd.Next(0, 60)),
                    ModifiedDate = now,
                    Modifiedby = "seed",
                    Alias = $"demo-product-{i}",
                    IsActive = true,
                    ViewCount = rnd.Next(0, 500),
                    OriginalPrice = basePrice * 1.1m
                });
            }

            db.TbProducts.AddRange(products);
            await db.SaveChangesAsync();

            // 5. Tạo ảnh sản phẩm
            var productImages = new List<TbProductImage>();
            foreach (var product in products)
            {
                // Ảnh chính
                productImages.Add(new TbProductImage
                {
                    ProductId = product.Id,
                    Image = product.Image,
                    IsDefault = true
                });

                // 1 ảnh phụ demo
                var secondary = productImagePool[rnd.Next(productImagePool.Length)];
                if (secondary == product.Image)
                {
                    secondary = productImagePool[(rnd.Next(productImagePool.Length) + 1) % productImagePool.Length];
                }
                productImages.Add(new TbProductImage
                {
                    ProductId = product.Id,
                    Image = secondary,
                    IsDefault = false
                });
            }

            db.TbProductImages.AddRange(productImages);
            await db.SaveChangesAsync();

            // 6. Tạo orders + order details
            var orders = new List<TbOrder>();
            var orderDetails = new List<TbOrderDetail>();

            for (int i = 1; i <= 30; i++)
            {
                var customerUser = users[rnd.Next(users.Count)];
                var orderDate = now.AddDays(-rnd.Next(0, 30));

                var order = new TbOrder
                {
                    Code = $"ORD{i:0000}",
                    CustomerName = customerUser.UserName ?? $"Customer {i}",
                    Phone = "0123456789",
                    Address = "Demo address",
                    TotalAmount = 0,
                    Quantity = 0,
                    CreatedBy = customerUser.UserName,
                    CreatedDate = orderDate,
                    ModifiedDate = orderDate,
                    Modifiedby = customerUser.UserName,
                    TypePayment = 1,
                    Email = customerUser.Email,
                    Status = 1
                };

                var itemCount = rnd.Next(1, 5);
                var usedProductIds = new HashSet<int>();

                for (int j = 0; j < itemCount; j++)
                {
                    var product = products[rnd.Next(products.Count)];
                    if (!usedProductIds.Add(product.Id))
                    {
                        continue;
                    }

                    var quantity = rnd.Next(1, 5);
                    var unitPrice = product.PriceSale ?? product.Price;

                    orderDetails.Add(new TbOrderDetail
                    {
                        Order = order,
                        Product = product,
                        ProductId = product.Id,
                        Price = unitPrice,
                        Quantity = quantity
                    });

                    order.Quantity += quantity;
                    order.TotalAmount += unitPrice * quantity;
                }

                orders.Add(order);
            }

            db.TbOrders.AddRange(orders);
            db.TbOrderDetails.AddRange(orderDetails);
            await db.SaveChangesAsync();

            // 7. Tạo bài viết (posts)
            var posts = new List<TbPost>();
            for (int i = 1; i <= 30; i++)
            {
                posts.Add(new TbPost
                {
                    Title = $"Demo Post {i}",
                    Description = $"Demo post {i} description",
                    Detail = $"This is the content of demo post {i}.",
                    Image = postImagePool[rnd.Next(postImagePool.Length)],
                    SeoTitle = $"Demo Post {i}",
                    SeoDescription = $"Demo post {i} description",
                    SeoKeywords = $"post{i}",
                    CreatedBy = "seed",
                    CreatedDate = now.AddDays(-rnd.Next(0, 90)),
                    ModifiedDate = now,
                    Modifiedby = "seed"
                });
            }
            db.TbPosts.AddRange(posts);
            await db.SaveChangesAsync();

            // 8. Tạo subscribe
            var subscribes = new List<TbSubscribe>();
            for (int i = 1; i <= 50; i++)
            {
                subscribes.Add(new TbSubscribe
                {
                    Email = $"subscriber{i}@example.com",
                    CreatedDate = now.AddDays(-rnd.Next(0, 90))
                });
            }
            db.TbSubscribes.AddRange(subscribes);
            await db.SaveChangesAsync();

            // 9. Tạo system setting cơ bản
            var settings = new List<TbSystemSetting>
            {
                new TbSystemSetting { SettingKey = "SiteName", SettingValue = "TT E-Commerce Demo", SettingDescription = "Tên website" },
                new TbSystemSetting { SettingKey = "Hotline", SettingValue = "0123 456 789", SettingDescription = "Số điện thoại liên hệ" },
                new TbSystemSetting { SettingKey = "EmailSupport", SettingValue = "support@example.com", SettingDescription = "Email hỗ trợ" },
                new TbSystemSetting { SettingKey = "Address", SettingValue = "Địa chỉ demo", SettingDescription = "Địa chỉ công ty" }
            };
            db.TbSystemSettings.AddRange(settings);
            await db.SaveChangesAsync();

            // 10. Tạo thống kê truy cập (ThongKe)
            var stats = new List<ThongKe>();
            for (int i = 0; i < 60; i++)
            {
                stats.Add(new ThongKe
                {
                    ThoiGian = now.Date.AddDays(-i),
                    SoTruyCap = rnd.Next(10, 500)
                });
            }
            db.ThongKes.AddRange(stats);
            await db.SaveChangesAsync();

            // 11. Tạo một số contact demo
            var contacts = new List<TbContact>();
            for (int i = 1; i <= 20; i++)
            {
                contacts.Add(new TbContact
                {
                    Name = $"Khách hàng {i}",
                    Email = $"customer{i}@example.com",
                    Website = "https://example.com",
                    Message = $"Đây là nội dung liên hệ demo số {i}.",
                    IsRead = i % 3 == 0,
                    CreatedBy = "seed",
                    CreatedDate = now.AddDays(-rnd.Next(0, 30)),
                    ModifiedDate = now,
                    Modifiedby = "seed"
                });
            }
            db.TbContacts.AddRange(contacts);
            await db.SaveChangesAsync();
        }
    }
}

