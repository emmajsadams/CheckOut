namespace CodeScreen.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     Checkout class to scan items according to the given <see cref="PricingRule"/> classes.
    /// </summary>
    public class Checkout
    {
        /// <summary>
        ///     The count of each item that has a discount associated with it. Reset to zero when the item 
        /// discount amount is reached.
        /// </summary>
        private IDictionary<string, int> discountItemCounts;

        /// <summary>
        ///     The pricing rules for items that can be scanned.
        /// </summary>
        private IDictionary<string, PricingRule> pricingRules;

        /// <summary>
        ///     The current total of all items scanned.
        /// </summary>
        private int total;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Checkout"/> class.
        /// </summary>
        /// <param name="pricingRules">
        ///     The pricing rules for this checkout.
        /// </param>
        public Checkout(IEnumerable<PricingRule> pricingRules)
        {
            this.pricingRules = new Dictionary<string, PricingRule>();
            this.discountItemCounts = new Dictionary<string, int>();

            foreach (var pricingRule in pricingRules)
            {
                this.pricingRules.Add(pricingRule.ItemName, pricingRule);

                // Add discount item to discountItemCounts
                if (pricingRule.DiscountAmount.HasValue)
                {
                    this.discountItemCounts.Add(pricingRule.ItemName, 0);
                }
            }
        }

        /// <summary>
        ///     The gets the current total of all items scanned according to the given pricingrules.
        /// </summary>
        /// <returns>
        ///     The <see cref="int" /> representing the current total.
        /// </returns>
        public int GetCurrentTotal()
        {
            return this.total;
        }

        /// <summary>
        ///     The scan.
        /// </summary>
        /// <param name="itemName">
        ///     The item name. Must have an associated PricingRule given in constructor.
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the given item name does not have a pricingRule associated with it.
        /// </exception>
        public void Scan(string itemName)
        {
            if (!this.pricingRules.ContainsKey(itemName))
            {
                throw new InvalidOperationException("No pricing rule was initialized for the given itemName");
            }

            var pricingRule = this.pricingRules[itemName];

            // and greater than or equal to
            if (pricingRule.DiscountAmount.HasValue)
            {
                int currentDiscountItemCount = this.discountItemCounts[itemName];

                // Update count and total
                currentDiscountItemCount++;
                this.total += pricingRule.Price;

                // Check if item at discount bulk
                if (currentDiscountItemCount == pricingRule.DiscountAmount.Value)
                {
                    // Remove non-discounted total
                    this.total -= pricingRule.DiscountAmount.Value * pricingRule.Price;

                    // Add discounted total
                    this.total += pricingRule.DiscountPrice.Value;

                    // Reset discount item count
                    currentDiscountItemCount = 0;
                }

                this.discountItemCounts[itemName] = currentDiscountItemCount;
            }
            else
            {
                // Item has no discount volume, simply add price for one item to total
                this.total += pricingRule.Price;
            }
        }
    }
}