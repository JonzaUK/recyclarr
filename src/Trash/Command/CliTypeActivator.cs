﻿using System;
using Autofac;

namespace Trash.Command
{
    internal static class CliTypeActivator
    {
        public static object ResolveType(IContainer container, Type typeToResolve)
        {
            var instance = container.Resolve(typeToResolve);
            if (instance.GetType().IsAssignableTo<IServiceCommand>())
            {
                var activeServiceProvider = container.Resolve<IActiveServiceCommandProvider>();
                activeServiceProvider.ActiveCommand = (IServiceCommand)instance;
            }

            return instance;
        }
    }
}
