using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Configuration;

namespace Nop.Plugin.OrderFullfillment.Shipwire
{
    class ShipwireSettings :ISettings
    {
        /// <summary>
        /// Gets or sets an amount of the additional handling charge
        /// </summary>
        public decimal AdditionalHandlingCharge { get; set; }
    }
}
