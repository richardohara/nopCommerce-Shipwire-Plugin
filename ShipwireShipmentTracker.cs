using System;
using System.Collections.Generic;
using System.Text;
using Nop.Plugin.OrderFullfillment.Shipwire.Services;
using Nop.Services.Shipping.Tracking;

namespace Nop.Plugin.OrderFullfillment.Shipwire
{
    class ShipwireShipmentTracker : IShipmentTracker
    {
        #region Fields

        private readonly ShipwireServices _shipwireService;

        #endregion

        #region Ctor

        public ShipwireShipmentTracker(ShipwireServices shipwireService)
        {
            _shipwireService = shipwireService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets if the current tracker can track the tracking number.
        /// </summary>
        /// <param name="trackingNumber">The tracking number to track.</param>
        /// <returns>True if the tracker can track, otherwise false.</returns>
        public virtual bool IsMatch(string trackingNumber)
        {
            //if (string.IsNullOrEmpty(trackingNumber))
                return false;
        }

        /// <summary>
        /// Gets an URL for a page to show tracking info (third party tracking page).
        /// </summary>
        /// <param name="trackingNumber">The tracking number to track.</param>
        /// <returns>URL of a tracking page.</returns>
        public virtual string GetUrl(string trackingNumber)
        {
            return $"https://www.ups.com/track?&tracknum={trackingNumber}";
        }

        /// </summary>
        /// <param name="trackingNumber">The tracking number to track</param>
        /// <returns>List of Shipment Events.</returns>
        public virtual IList<ShipmentStatusEvent> GetShipmentEvents(string trackingNumber)
        {
            var result = new List<ShipmentStatusEvent>();

            if (string.IsNullOrEmpty(trackingNumber))
                return result;

            //result.AddRange(_shipwireService.GetShipmentEvents(trackingNumber));

            return result;
        }

        #endregion
    }
}
