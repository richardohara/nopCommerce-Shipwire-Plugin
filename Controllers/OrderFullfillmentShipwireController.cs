using System;
using System.Collections.Generic;
using System.Text;
//using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Plugin.OrderFullfillment.Shipwire.Models;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Shipping;
using Nop.Web.Controllers;
using Nop.Web.Factories;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Services.Logging;

namespace Nop.Plugin.OrderFullfillment.Shipwire.Controllers
{
    class OrderFullfillmentShipwireController : CheckoutController
    {
        #region Fields

        private readonly AddressSettings _addressSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly IAddressAttributeParser _addressAttributeParser;
        private readonly IAddressService _addressService;
        private readonly ICheckoutModelFactory _checkoutModelFactory;
        private readonly ICountryService _countryService;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;
        private readonly IPaymentPluginManager _paymentPluginManager;
        private readonly IPaymentService _paymentService;
        private readonly IShippingService _shippingService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IStoreContext _storeContext;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly OrderSettings _orderSettings;
        private readonly PaymentSettings _paymentSettings;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly ShippingSettings _shippingSettings;

        #endregion

        #region Ctor

        public OrderFullfillmentShipwireController(AddressSettings addressSettings,
            CustomerSettings customerSettings,
            IAddressAttributeParser addressAttributeParser,
            IAddressService addressService,
            ICheckoutModelFactory checkoutModelFactory,
            ICountryService countryService,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            ILogger logger,
            IOrderProcessingService orderProcessingService,
            IOrderService orderService,
            IPaymentPluginManager paymentPluginManager,
            IPaymentService paymentService,
            IShippingService shippingService,
            IShoppingCartService shoppingCartService,
            IStateProvinceService stateProvinceService,
            IStoreContext storeContext,
            IWebHelper webHelper,
            IWorkContext workContext,
            OrderSettings orderSettings,
            PaymentSettings paymentSettings,
            RewardPointsSettings rewardPointsSettings,
            ShippingSettings shippingSettings) : base(addressSettings,
            customerSettings,
            addressAttributeParser,
            addressService,
            checkoutModelFactory,
            countryService,
            customerService,
            genericAttributeService,
            localizationService,
            logger,
            orderProcessingService,
            orderService,
            paymentPluginManager,
            paymentService,
            shippingService,
            shoppingCartService,
            stateProvinceService,
            storeContext,
            webHelper,
            workContext,
            orderSettings,
            paymentSettings,
            rewardPointsSettings,
            shippingSettings)
        { }

        #endregion


        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            var model = new ShipwireModel
            {
                //UseSandbox = payPalStandardPaymentSettings.UseSandbox,
                //BusinessEmail = payPalStandardPaymentSettings.BusinessEmail,
                //PdtToken = payPalStandardPaymentSettings.PdtToken,
                //PassProductNamesAndTotals = payPalStandardPaymentSettings.PassProductNamesAndTotals,
                //AdditionalFee = payPalStandardPaymentSettings.AdditionalFee,
                //AdditionalFeePercentage = payPalStandardPaymentSettings.AdditionalFeePercentage,
                //ActiveStoreScopeConfiguration = storeScope
            };

            return View("~/Plugins/OrderFullfillment.Shipwire/Views/Configure.cshtml", model);
        }

        public override IActionResult ShippingMethod()
        {
            return View();
        }
        #endregion
    }
}
