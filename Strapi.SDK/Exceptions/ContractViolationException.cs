using System;

namespace Strapi_SDK
{
    public class ContractViolationException : Exception
    {
        public ContractViolationException(string message) : base(message)
        {

        }
    }
}