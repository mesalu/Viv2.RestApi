using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Viv2.API.Core.ProtoEntities;

#nullable enable

namespace Viv2.API.AppInterface.Dto
{
    public class ControllerDto
    {
        public static ControllerDto From([NotNull] IController controller)
        {
            return new ControllerDto
            {
                Id = controller.Id,
                OwnerId = controller.Owner?.Guid,
                EnvironmentIds = controller.Environments?.Select(e => e.Id)
            };
        }
        
        public Guid Id { get; init; }
        public Guid? OwnerId { get; init; }
        public IEnumerable<Guid?>? EnvironmentIds { get; init; }
    }
}