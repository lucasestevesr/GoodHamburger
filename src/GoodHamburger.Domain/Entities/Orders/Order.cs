using GoodHamburger.Domain.Entities.Base;
using GoodHamburger.Domain.Entities.Products;

namespace GoodHamburger.Domain.Entities.Orders
{
    public sealed class Order : BaseEntity
    {
        private readonly List<OrderItem> _items = new();

        public long OrderNumber { get; set; }

        public decimal SubTotal { get; private set; }

        public decimal DiscountRate { get; private set; }

        public decimal Total { get; private set; }

        public Guid CreatedBy { get; set; }

        public OrderStatus Status { get; set; }

        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        private void ValidateNewItem(Product product, int quantity)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            if (quantity <= 0)
                throw new DomainException("Quantidade deve ser maior que zero.");

            if (!product.IsActive)
                throw new DomainException("Produto inativo não pode ser adicionado ao pedido.");

            if (_items.Any(i => i.ProductId == product.Id))
                throw new DomainException("Não pode haver itens duplicados no pedido.");

            if (_items.Any(i => i.Category == product.Category))
                throw new DomainException($"O pedido só pode conter 1 item da categoria '{product.Category}'.");
        }

        public void AddItem(Product product, int quantity)
        {
            ValidateNewItem(product, quantity);

            var orderItem = new OrderItem
            {
                Order = this,
                OrderId = Id,
                ProductId = product.Id,
                Product = product,
                Category = product.Category,
                ProductPrice = product.Price
            };

            orderItem.ValidateQuantity(quantity, product);

            _items.Add(orderItem);
            CalculateTotalPrice();
        }

        private OrderItem EnsureItemExists(Guid productId)
        {
            var item = _items.SingleOrDefault(i => i.ProductId == productId);
            if (item is null)
                throw new DomainException("Item não encontrado no pedido.");

            return item;
        }

        public void UpdateItemQuantity(Guid productId, int quantity)
        {
            var item = EnsureItemExists(productId);

            item.ValidateQuantity(quantity, item.Product);
            CalculateTotalPrice();
        }

        public void RemoveItem(Guid productId)
        {
            var item = EnsureItemExists(productId);

            _items.Remove(item);
            CalculateTotalPrice();
        }

        public void CalculateTotalPrice()
        {
            SubTotal = _items.Sum(i => i.LineTotal);
            DiscountRate = CalculateDiscountRate();
            Total = SubTotal * (1 - DiscountRate);
        }

        private decimal CalculateDiscountRate()
        {
            var hasBurger = _items.Any(i => i.Category == ProductCategory.Burger);
            var hasSide = _items.Any(i => i.Category == ProductCategory.Side);
            var hasDrink = _items.Any(i => i.Category == ProductCategory.Drink);

            if (hasBurger && hasSide && hasDrink) return 0.20m;
            if (hasBurger && hasDrink) return 0.15m;
            if (hasBurger && hasSide) return 0.10m;

            return 0m;
        }
    }
}
