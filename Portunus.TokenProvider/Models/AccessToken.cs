using System;
using System.Dynamic;

namespace Portunus.TokenProvider.Models
{
    public class AccessToken
    {
        public AccessToken()
        {
            TokenId = Guid.NewGuid();
        }

        public DateTime ExpireOn { get; set; }

        public ExpandoObject Payload { get; set; }

        public Guid TokenId { get; set; }
    }
}