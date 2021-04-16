using Viv2.API.Core.Interfaces;

namespace Viv2.API.AppInterface.Ports
{
    public class BasicPresenter <TResponse> : IOutboundPort<TResponse>
    { 
        public TResponse Response { get; private set; }

        public void Handle(TResponse response)
        {
            Response = response;
        }
    }
}