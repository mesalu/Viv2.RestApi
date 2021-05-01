using System;
using System.Diagnostics.CodeAnalysis;
using Viv2.API.Core.ProtoEntities;

#nullable enable

namespace Viv2.API.AppInterface.Dto
{
    /// <summary>
    /// Represents a sample in transit *out* of the controller.
    /// Reduced to improve transmission / mask out potential cyclical references.
    /// </summary>
    public class SampleDto
    {
        public static SampleDto From([NotNull] IEnvDataSample sample)
        {
            var dto = new SampleDto
            {
                Environment = sample.Environment?.Id,
                Occupant = sample.Occupant?.Id,
                Captured = sample.Captured ?? DateTime.MinValue,
                HotGlass = sample.HotGlass,
                HotMat = sample.HotMat,
                MidGlass = sample.MidGlass,
                ColdGlass = sample.ColdGlass,
                ColdMat = sample.ColdMat
            };

            return dto;
        }
        
        public Guid? Environment { get; init; }
        public int? Occupant { get; init; }
        
        public DateTime Captured { get; init; }
        public double? HotGlass { get; init; }
        public double? HotMat { get; init; }
        public double? MidGlass { get; init; }
        public double? ColdGlass { get; init; }
        public double? ColdMat { get; init; }
    }
}