using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;
using System;

namespace Fanex.Affiliate.Admin.Core.UnitTests.TestHelper
{
    public class SimpleLifetimeScopeProvider : ILifetimeScopeProvider
    {
        private readonly IContainer _container;
        private ILifetimeScope _scope;

        public SimpleLifetimeScopeProvider(IContainer container)
        {
            this._container = container;
        }

        public ILifetimeScope ApplicationContainer
        {
            get { return this._container; }
        }

        public void EndLifetimeScope()
        {
            if (this._scope != null)
            {
                this._scope.Dispose();
                this._scope = null;
            }
        }

        public ILifetimeScope GetLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            if (this._scope == null)
            {
                this._scope = (configurationAction == null)
                       ? this.ApplicationContainer.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag)
                       : this.ApplicationContainer.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag, configurationAction);
            }

            return this._scope;
        }
    }
}
