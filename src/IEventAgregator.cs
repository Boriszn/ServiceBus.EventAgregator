using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace ServiceBus.EventAgregator
{
    /// <summary>
    /// Contains logic to trigger event listening And logic to asign event subscribers 
    /// </summary>
    /// <seealso cref="IEventAgregator" />
    public interface IEventAgregator
    {
        /// <summary>
        /// Starts the agregating.
        /// </summary>
        Task StartAgregating();

        /// <summary>
        /// First message handler example.
        /// </summary>
        /// <param name="message">The message.</param>
        bool DoFirstHandler(Message message);

        /// <summary>
        /// Second message handler example.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        bool DoSecondHandler(Message message);
    }
}