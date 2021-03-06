using System;
using System.Collections.Generic;
using Symbiote.Jackalope;
using Symbiote.Jackalope.Control;

namespace Symbiote.Telepathy
{
    public class SubscriptionControlMessagehandler : IMessageHandler<SubscriptionControlMessage>
    {
        private ISubscriptionManager _subscriptionManager;
        private Dictionary<SubscriptionAction, Action<ISubscriptionManager, SubscriptionControlMessage>> _controlActions
            = new Dictionary<SubscriptionAction, Action<ISubscriptionManager, SubscriptionControlMessage>>()
                  {
                      {SubscriptionAction.Start, 
                          (x,y) =>
                              {
                                  x.StartSubscription(y.Queue);
                              }},
                      {SubscriptionAction.StartAll, 
                          (x,y) =>
                              {
                                  x.StartAllSubscriptions();
                              }},
                      {SubscriptionAction.Stop, 
                          (x,y) =>
                              {
                                  x.StopSubscription(y.Queue);
                              }},
                      {SubscriptionAction.StopAll, 
                          (x,y) =>
                              {
                                  x.StopAllSubscriptions();
                              }},
                      {SubscriptionAction.NoOp, 
                          (x,y) =>
                              {
                            
                              }},
                  };

        public void Process(SubscriptionControlMessage message, IResponse response)
        {
            var reply = new ServiceControlResponse();
            try
            {
                _controlActions[message.Action](_subscriptionManager, message);
                reply.Success = true;
                response.Reply(reply);
            }
            catch (Exception ex)
            {
                reply.ExceptionOccurred = true;
                reply.Exceptions.Add(ex);
                response.Reply(reply);
                throw;
            }
        }

        public SubscriptionControlMessagehandler(ISubscriptionManager manager)
        {
            _subscriptionManager = manager;
        }
    }
}