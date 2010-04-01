using System;
using System.Text;
using Symbiote.Core.Extensions;

namespace Symbiote.Relax.Impl
{
    public class CouchUri : ICloneable
    {
        private StringBuilder _builder = new StringBuilder();
        private bool _hasArguments = false;

        public static CouchUri Build(string prefix, string server, int port, string database)
        {
            return new CouchUri(prefix, server, port, database);
        }

        public CouchUri ListAll() 
        {
            _builder.Append("/_all_docs");
            return this;
        }

        public CouchUri Changes(Feed feed, int since)
        {
            _builder.Append("/_changes");

            if (feed != Feed.Simple)
            {
                _builder.AppendFormat("?feed={0}",
                                      feed == Feed.Continuous
                                          ? "continuous"
                                          : "longpoll");
            }
            _builder.AppendFormat("&since={0}", since);
            _hasArguments = true;
            return this;
        }

        public CouchUri Design(string designDocumentName)
        {
            _builder.AppendFormat("/_design/{0}", designDocumentName);
            return this;
        }

        public CouchUri View(string viewName)
        {
            _builder.AppendFormat("/_view/{0}", viewName);
            return this;
        }

        public CouchUri List(string listName)
        {
            _builder.AppendFormat("/_list/{0}", listName);
            return this;
        }

        public CouchUri Descending()
        {
            _builder.AppendFormat("{0}descending=true",
                                  _hasArguments ? "&" : "?");

            if (!_hasArguments)
                _hasArguments = true;

            return this;
        }

        public CouchUri Limit(int limit)
        {
            _builder.AppendFormat("{0}limit={1}",
                                  _hasArguments ? "&" : "?", limit);

            if (!_hasArguments)
                _hasArguments = true;

            return this;
        }

        public CouchUri Skip(int number)
        {
            _builder.AppendFormat("{0}skip={1}",
                                  _hasArguments ? "&" : "?", number);

            if (!_hasArguments)
                _hasArguments = true;

            return this;
        }

        public CouchUri Format(string format)
        {
            _builder.AppendFormat("{0}format={1}",
                                  _hasArguments ? "&" : "?", format);

            if (!_hasArguments)
                _hasArguments = true;

            return this;
        }

        public CouchUri KeyAndRev<TKey, TRev>(TKey key, TRev rev)
        {
            if (!_hasArguments)
                _hasArguments = true;

            _builder.AppendFormat("/{0}?rev={1}",
                                  key.ToString().TrimStart('"').TrimEnd('"'),
                                  rev.ToString().TrimStart('"').TrimEnd('"'));
            return this;
        }

        public CouchUri Key<TKey>(TKey key)
        {
            if (!_hasArguments)
                _hasArguments = true;

            _builder.AppendFormat("/{0}", key.ToString().TrimStart('"').TrimEnd('"'));

            return this;
        }

        public CouchUri ByRange<TKey>(TKey start, TKey end)
        {
            _builder.AppendFormat("{0}startkey={1}&endkey={2}",
                                  _hasArguments ? "&" : "?",
                                  start.ToString().TrimStart('"').TrimEnd('"'),
                                  end.ToString().TrimStart('"').TrimEnd('"'));

            if (!_hasArguments)
                _hasArguments = true;

            return this;
        }

        public CouchUri IncludeDocuments()
        {
            _builder.AppendFormat("{0}include_docs=true", _hasArguments
                                                              ? "&"
                                                              : "?");

            if (!_hasArguments)
                _hasArguments = true;

            return this;
        }

        public CouchUri BulkInsert()
        {
            _builder.Append("/_bulk_docs");
            return this;
        }

        public CouchUri(string prefix, string server, int port, string database)
        {
            _builder
                .AppendFormat(@"{0}://{1}:{2}/{3}", prefix, server, port, database);
        }

        protected CouchUri(string content, bool hasArgs)
        {
            _builder.Append(content);
            _hasArguments = hasArgs;
        }

        public object Clone()
        {
            return new CouchUri(ToString(), _hasArguments);
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}