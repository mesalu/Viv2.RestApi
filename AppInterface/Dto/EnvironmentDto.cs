using System;
using System.Diagnostics.CodeAnalysis;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.AppInterface.Dto
{
    public class EnvironmentDto
    {
        public static EnvironmentDto From([NotNull] IEnvironment env)
        {
            return new EnvironmentDto
            {
                Id = env.Id?.ToString(),
                ControllerId = env.Controller?.Id.ToString(),
                InhabitantId = env.Inhabitant?.Id,
                Model = env.Model,
                Descr = env.Descr
            };
        }
        
         public string? Id { get; set; }
        public string? ControllerId { get; set; }
        public int? InhabitantId { get; set; }
        public string? Model { get; set; }
        public string? Descr { get; set; }
    }
}