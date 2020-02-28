using System;
using System.Collections.Generic;
using System.Text;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.OrderFullfillment.Shipwire.Models
{
    class ShipwireModel : BaseNopModel
    {
        public bool UseSandbox { get; set; }
    }
}
