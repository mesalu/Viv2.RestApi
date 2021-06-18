using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.Adapters
{
    public interface ISampleStore
    {
        Task<IList<IEnvDataSample>> GetRangeByUser([NotNull] IUser user, DateTime start, DateTime end);
        Task<IList<IEnvDataSample>> GetRangeByEnv([NotNull] IEnvironment env, DateTime start, DateTime end);
        Task<IList<IEnvDataSample>> GetRangeByPet([NotNull] IPet pet, DateTime start, DateTime end);
    }
}