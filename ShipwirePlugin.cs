using System;
using Nop.Core;
using Nop.Services.Plugins;

namespace Nop.Plugin.OrderFullfillment.Shipwire
{
    public class Shipwire : BasePlugin
    {
        #region Fields

        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor
        public Shipwire(IWebHelper webHelper)
        {
            _webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/OrderFullfillmentShipwireController/Configure";
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        public override void Install()
        {
            base.Install();
            //PluginManager.MarkPluginAsInstalled(PluginDescriptor.SystemName);
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        public override void Uninstall()
        {
            base.Uninstall();
            //PluginManager.MarkPluginAsUninstalled(PluginDescriptor.SystemName);
        }

        #endregion
    }
}
