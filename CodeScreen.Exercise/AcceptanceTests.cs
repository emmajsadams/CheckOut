using System.Collections.Generic;
using Xunit;

namespace CodeScreen.Models
{
    using System;
    using System.Threading;

    public class AcceptanceTests
    {
        private IEnumerable<PricingRule> pricingRules =
            new List<PricingRule>() { 
                new PricingRule() { ItemName = "Magical Sword", Price = 350 },
                new PricingRule() { ItemName = "Ice Staff", Price = 330 },
                new PricingRule() { ItemName = "Golden Apple", Price = 20, DiscountAmount = 5, DiscountPrice = 75 },
                new PricingRule() { ItemName = "Strength Potion", Price = 80, DiscountAmount = 3, DiscountPrice = 210 }};

        [Fact]
        public void CanPriceOneItem()
        {
            var checkout = new Checkout(pricingRules);

            checkout.Scan("Magical Sword");

            Assert.Equal(350, checkout.GetCurrentTotal());
        }

        [Fact]
        public void CanPriceMultipleDifferentItems()
        {
            var checkout = new Checkout(pricingRules);

            checkout.Scan("Magical Sword");
            checkout.Scan("Ice Staff");

            Assert.Equal(680, checkout.GetCurrentTotal());
        }

        [Fact]
        public void CanCalculateDiscount()
        {
            var checkout = new Checkout(pricingRules);

            checkout.Scan("Strength Potion");
            checkout.Scan("Strength Potion");
            checkout.Scan("Strength Potion");

            Assert.Equal(210, checkout.GetCurrentTotal());
        }

        [Fact]
        public void CanCalculateDiscountWithOneMoreDiscountItem()
        {
            var checkout = new Checkout(pricingRules);

            checkout.Scan("Strength Potion");
            checkout.Scan("Strength Potion");
            checkout.Scan("Strength Potion");
            checkout.Scan("Strength Potion");


            Assert.Equal(290, checkout.GetCurrentTotal());
        }

        [Fact]
        public void CanCalculateDiscountWithOneNonDiscountItem()
        {
            var checkout = new Checkout(pricingRules);

            checkout.Scan("Ice Staff");
            checkout.Scan("Strength Potion");
            checkout.Scan("Strength Potion");
            checkout.Scan("Strength Potion");

            Assert.Equal(540, checkout.GetCurrentTotal());
        }

        [Fact]
        public void CanCalculateTwoDiscountsFromDifferentItems()
        {
            var checkout = new Checkout(pricingRules);

            checkout.Scan("Golden Apple");
            checkout.Scan("Golden Apple");
            checkout.Scan("Golden Apple");
            checkout.Scan("Golden Apple");
            checkout.Scan("Golden Apple");
            checkout.Scan("Strength Potion");
            checkout.Scan("Strength Potion");
            checkout.Scan("Strength Potion");

            Assert.Equal(285, checkout.GetCurrentTotal());
        }


        [Fact]
        public void WillThrowIllegalOperationExceptionForItemNameWithoutPricingRule()
        {
            var checkout = new Checkout(pricingRules);

            Exception ex = Assert.Throws<InvalidOperationException>(() => checkout.Scan("The Bag of Holding"));

            Assert.Equal("No pricing rule was initialized for the given itemName", ex.Message);
        }

    }
}
