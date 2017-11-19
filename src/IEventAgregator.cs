using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace ServiceBusTopics
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEventAgregator
    {
        /// <summary>
        /// Starts the agregating.
        /// </summary>
        Task StartAgregating();

        /// <summary>
        /// Firsts the handler.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        bool DoFirstHandler(Message message);

        /// <summary>
        /// Does the second handler.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        bool DoSecondHandler(Message message);
    }
}