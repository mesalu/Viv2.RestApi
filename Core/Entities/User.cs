using System;
using System.Collections.Generic;

namespace Viv2.API.Core.Entities
{
    /// <summary>
    /// Represents a user in an infrastructure agnostic manner.
    /// E.g., nothing about this model will be specific to EF or MS Identity.
    /// </summary>
    public class User
    {
        public User() : base()
        {
            Pets = new HashSet<Pet>();
            Environments = new HashSet<Environment>();
            RefreshTokens = new HashSet<RefreshToken>();
        }
        
        public string Name { get; set; }
        public Guid Id { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }
        public virtual ICollection<Environment> Environments { get; set; }
    }
}
