using CSharpSPDH.Models;
using System;

namespace CSharpSPDH
{
    /// <summary>
    /// SPDH Class for Parse and Build Utilities
    /// </summary>
    public class Spdh
    {
        /// <summary>
        /// Parses bytes received to TMessage object
        /// </summary>
        /// <typeparam name="TMessage">deriving class from Message Model</typeparam>
        /// <param name="messageBytes">received messages bytes to be parsed</param>
        /// <returns>TMessage instance</returns>
        public virtual TMessage Parse<TMessage>(byte[] messageBytes) where TMessage : Message => throw new NotImplementedException();

        /// <summary>
        /// Generates byte array of message from TMessage object
        /// </summary>
        /// <typeparam name="TMessage">deriving class from Message Model</typeparam>
        /// <param name="message">message object</param>
        /// <returns>message builded bytes</returns>
        public virtual byte[] Build<TMessage>(TMessage message) where TMessage : Message => throw new NotImplementedException();
    }
}
