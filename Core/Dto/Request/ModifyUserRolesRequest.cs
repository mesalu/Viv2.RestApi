using System;
using System.Collections;
using System.Collections.Generic;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;

namespace Viv2.API.Core.Dto.Request
{
    public class ModifyUserRolesRequest : IUseCaseRequest<BlankResponse>
    {
        public enum ModificationMode
        {
            Set,
            Add
        }
        
        public Guid UserId;
        public ModificationMode Mode { get; set; }
        public IEnumerable<string> DesiredRoles;
    }
}