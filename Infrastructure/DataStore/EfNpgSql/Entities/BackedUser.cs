using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Viv2.API.Core.Entities;
using Environment = Viv2.API.Core.Entities.Environment;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities
{
    public class BackedUser : IdentityUser, IBackedUser
    {
        public BackedUser() : base()
        {
            Pets = new HashSet<Pet>();
            Environments = new HashSet<Environment>();
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public virtual ICollection<RefreshToken> RefreshTokens { get; private set; }
        public virtual ICollection<Pet> Pets { get; private set; }
        public virtual ICollection<Environment> Environments { get; private set; }

        public User ToCoreUser()
        {
            return new User
            {
                RefreshTokens = this.RefreshTokens,
                Pets = this.Pets,
                Environments = this.Environments,
                Id = Guid.Parse(this.Id),
                Name = this.UserName,
            };
        }

        public void ApplyCoreUser(User user)
        {
            // Potentially lossy, for now trust that `user` is the source of truth.
            RefreshTokens = user.RefreshTokens;
            Pets = user.Pets;
            Environments = user.Environments;
            Id = user.Id.ToString();
            UserName = user.Name;
        }
    }
}
