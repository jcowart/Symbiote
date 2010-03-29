﻿using System;
using Machine.Specifications;
using Symbiote.Relax.Impl;

namespace Relax.Tests.Repository
{
    public abstract class with_delete_document_command : with_document_repository
    {
        protected static Guid id;

        private Establish context = () =>
                                        {
                                            id = Guid.NewGuid();
                                            uri = new CouchURI("http", "localhost", 5984, "testdocument").Key(id);
                                            commandMock.Setup(x => x.Delete(couchUri));
                                            WireUpCommandMock(commandMock.Object);
                                        };
    }
}