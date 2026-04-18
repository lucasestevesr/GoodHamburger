using GoodHamburger.Domain.Entities.Base;
using GoodHamburger.Domain.Entities.Products;

namespace GoodHamburger.Domain.Entities.Orders
{
    /// <summary>
    /// Representa uma ordem de compra na hamburgueria.
    /// </summary>
    public sealed class Order : BaseEntity
    {
        public long OrderNumber { get; set; }

        /// <summary>
        /// Soma dos itens do pedido antes do desconto.
        /// </summary>
        public decimal SubTotal { get; private set; }

        /// <summary>
        /// Percentual de desconto aplicado ao pedido, calculado com base nas regras de negócio.
        /// </summary>
        public decimal DiscountRate { get; private set; }
        
        /// <summary>
        /// Preço total do pedido após aplicação de descontos.
        /// </summary>
        public decimal Total { get; private set; }

        public Guid CreatedBy { get; set; }

        public OrderStatus Status { get; set; }

        public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

        /// <summary>
        /// Regras de negócio para criação de pedido;
        /// - não permite duplicidade do mesmo produto;
        /// - permite no máximo 1 item por categoria (Burger, Side, Drink);
        /// - quantidade deve ser maior que zero;
        /// - produto deve estar ativo.
        /// </summary>
        /// <param name="product">Produto a ser adicionado.</param>
        /// <param name="quantity">Quantidade desejada de cada item.</param>
        /// <exception cref="ArgumentNullException">Quando <paramref name="product"/> é nulo.</exception>
        /// <exception cref="DomainException">Quando alguma regra de negócio é violada.</exception>
        public void AddItem(Product product, int quantity)
        {
            if (product is null) 
                throw new ArgumentNullException(nameof(product));
            if (!product.IsActive)
                throw new DomainException("Produto inativo não pode ser adicionado ao pedido.");           
           
            if (Items.Any(i => i.ProductId == product.Id))
                throw new DomainException("Não pode haver itens duplicados no pedido.");
            
            if (Items.Any(i => i.Category == product.Category))
                throw new DomainException($"O pedido só pode conter 1 item da categoria '{product.Category}'.");

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Product = product,

                Category = product.Category,
                ProductPrice = product.Price,
            };

            orderItem.ChangeQuantity(quantity, product);

            CalculateTotalPrice();
        }

        /// <summary>
        /// Atualiza a quantidade de um item existente no pedido.
        /// </summary>
        /// <param name="productId">Identificador do produto.</param>
        /// <param name="quantity">Nova quantidade.</param>
        /// <exception cref="DomainException">
        /// Quando o item não existe no pedido ou quando a quantidade é inválida.
        /// </exception>
        public void UpdateItemQuantity(Guid productId, int quantity)
        {
            var item = Items.SingleOrDefault(i => i.ProductId == productId);
            if (item is null)
                throw new DomainException("Item não encontrado no pedido.");

            item.ChangeQuantity(quantity);
            CalculateTotalPrice();
        }

        /// <summary>
        /// Remove um item do pedido.
        /// </summary>
        /// <param name="productId">Identificador do produto.</param>
        /// <exception cref="DomainException">Quando o item não existe no pedido.</exception>
        public void RemoveItem(Guid productId)
        {
            var item = Items.SingleOrDefault(i => i.ProductId == productId);
            if (item is null)
                throw new DomainException("Item não encontrado no pedido.");

            Items.Remove(item);
            CalculateTotalPrice();
        }

        /// <summary>
        /// Calcula <see cref="SubTotal"/>, <see cref="DiscountRate"/> e <see cref="Total"/>
        /// com base nos itens atuais do pedido.
        /// Deve ser chamado após qualquer alteração em <see cref="Items"/>.
        /// </summary>
        public void CalculateTotalPrice()
        {
            SubTotal = Items.Sum(i => i.LineTotal);
            DiscountRate = CalculateDiscountRate();
            Total = SubTotal * (1 - DiscountRate);
        }

        /// <summary>
        /// Calcula o percentual de desconto do pedido.
        /// Caso nenhuma regra aplique, retorna 0.
        /// </summary>
        private decimal CalculateDiscountRate()
        {
            var hasBurger = Items.Any(i => i.Category == ProductCategory.Burger);
            var hasSide = Items.Any(i => i.Category == ProductCategory.Side);
            var hasDrink = Items.Any(i => i.Category == ProductCategory.Drink);

            if (hasBurger && hasSide && hasDrink) return 0.20m;
            if (hasBurger && hasDrink) return 0.15m;
            if (hasBurger && hasSide) return 0.10m;

            return 0m;
        }
    }
}
