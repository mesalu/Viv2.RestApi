using System;
using System.Collections.Generic;

namespace Viv2.API.Core.ProtoEntities
{
    /// <summary>
    /// Represents a user in an infrastructure agnostic manner.
    /// E.g., nothing about this model will be specific to EF or MS Identity.
    /// </summary>
    public interface IUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid Guid { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; }
        public ICollection<IPet> Pets { get; }
        public ICollection<IEnvironment> Environments { get; }
    }
}
