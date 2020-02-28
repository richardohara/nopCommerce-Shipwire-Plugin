using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.OrderFullfillment.Shipwire.Domain
{
    class DeliveryServiceAttribute : Attribute
    {
        #region Ctor

        public DeliveryServiceAttribute(string codeValue)
        {
            Code = codeValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a code value
        /// </summary>
        public string Code { get; }

        #endregion
    }
}
