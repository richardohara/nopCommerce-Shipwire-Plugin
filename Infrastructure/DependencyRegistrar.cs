using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.OrderFullfillment.Shipwire.Controllers;
using Nop.Web.Controllers;

namespace Nop.Plugin.OrderFullfillment.Shipwire.Infrastructure
{
    class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        { 
            builder.RegisterType<OrderFullfillmentShipwireController>().As<CheckoutController>().InstancePerLifetimeScope();
            Console.WriteLine("Shipwire registered!!");
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order => 1;
    }
}
