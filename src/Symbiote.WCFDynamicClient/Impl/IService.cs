using System;
using System.ServiceModel.Description;

namespace Symbiote.WCFDynamicClient.Impl
{
    public interface IService<TContract> : IDisposable
        where TContract : class
    {
        ClientCredentials ClientCredentials { get; }
        void Call(Action<TContract> call);
        TResult Call<TResult>(Func<TContract, TResult> call);
        void Close();
    }
}