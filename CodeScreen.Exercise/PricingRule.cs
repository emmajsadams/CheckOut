namespace CodeScreen.Models
{
    public class PricingRule
    {
        // The name of the item
        public string ItemName { get; set; }

        // Cost for a single item in copper coins
        public int Price { get; set; }

        // The number of items that must be purchased
        // to receive the discounted price
        public int? DiscountAmount { get; set; }

        // The discounted price for the DiscountAmount items
        public int? DiscountPrice { get; set; }
    }
}
