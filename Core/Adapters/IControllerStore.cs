using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.Adapters
{
    public interface IControllerStore
    {
        /// <summary>
        /// Finds and returns an instance of IController that matches the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IController> GetById(Guid id);

        /// <summary>
        /// Reassigns the controller / environment relationship of the given environment to
        /// the given controller.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public Task ReParentEnvironment([NotNull] IController controller, [NotNull] IEnvironment environment);
    }
}