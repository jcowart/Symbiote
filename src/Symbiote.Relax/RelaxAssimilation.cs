﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Symbiote.Core;
using Symbiote.Relax.Impl;

namespace Symbiote.Relax
{
    public static class RelaxAssimilation
    {
        public static IAssimilate Relax(this IAssimilate assimilate, Action<CouchConfigurator> configure)
        {
            var config = new CouchConfigurator();
            configure(config);

            assimilate
                .Dependencies(c =>
                              {
                                  var configuration = config.GetConfiguration();
                                  c.For<ICouchConfiguration>().Use(configuration);

                                  if (configuration.Cache)
                                  {
                                      c.For<IDocumentRepository>().Use<CachedDocumentRepository>();
                                      c.For<IDocumentRepository<Guid, string>>().Use<CachedDocumentRepository>();
                                      c.For(typeof (IDocumentRepository<>)).Use(typeof (CachedDocumentRepository<>));
                                  }
                                  else
                                  {
                                      c.For<IDocumentRepository>().Use<DocumentRepository>();
                                      c.For<IDocumentRepository<Guid, string>>().Use<DocumentRepository>();
                                      c.For(typeof (IDocumentRepository<>)).Use(typeof (DocumentRepository<>));
                                  }
                              });

            return assimilate;
        }
    }
}