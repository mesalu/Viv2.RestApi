using System;

#nullable enable

namespace Viv2.API.Core.ProtoEntities
{
    public interface IEnvDataSample
    {
        /// <summary>
        /// The environment where the sample was taken. 
        /// </summary>
        public IEnvironment Environment { get; }
        
        /// <summary>
        /// The occupant of the environment at the time of the sample.
        /// </summary>
        public IPet? Occupant { get; }
        
        public DateTime Captured { get; }
        public double HotGlass { get; }
        public double HotMat { get; }
        public double MidGlass { get; }
        public double ColdGlass { get; }
        public double ColdMat { get; }
    }
}