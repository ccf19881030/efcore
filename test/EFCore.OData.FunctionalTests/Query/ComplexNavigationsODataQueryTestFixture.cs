// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using Microsoft.AspNet.OData.Builder;
using Microsoft.EntityFrameworkCore.TestModels.ComplexNavigationsModel;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;

namespace Microsoft.EntityFrameworkCore.Query
{
    public class ComplexNavigationsODataQueryTestFixture : ComplexNavigationsQuerySqlServerFixture//, IODataQueryTestFixture
    {
        private IHost _selfHostServer = null;

        protected override string StoreName { get; } = "ODataComplexNavigations";

        public ComplexNavigationsODataQueryTestFixture()
        {
            var controllers = new Type[]
            {
                typeof(LevelOneController),
                typeof(LevelTwoController),
                typeof(LevelThreeController),
                typeof(LevelFourController),
            };

            (BaseAddress, ClientFactory, _selfHostServer)
                = ODataQueryTestFixtureInitializer.Initialize<ComplexNavigationsODataContext>(StoreName, controllers, GetEdmModel());
        }

        private static IEdmModel GetEdmModel()
        {
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Level1>("LevelOne");
            modelBuilder.EntitySet<Level2>("LevelTwo");
            modelBuilder.EntitySet<Level3>("LevelThree");
            modelBuilder.EntitySet<Level4>("LevelFour");

            return modelBuilder.GetEdmModel();
        }

        public string BaseAddress { get; private set; }

        public IHttpClientFactory ClientFactory { get; private set; }

        public override void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_selfHostServer != null)
                {
                    _selfHostServer.StopAsync();
                    _selfHostServer.WaitForShutdownAsync();
                    _selfHostServer = null;
                }
            }
        }
    }
}
