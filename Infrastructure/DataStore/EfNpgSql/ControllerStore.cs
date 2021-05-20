using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.ProtoEntities;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Contexts;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities;
using Environment = Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Environment;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql
{
    public class ControllerStore : IControllerStore
    {
        private readonly DataContext _context;

        public ControllerStore(DataContext context)
        {
            _context = context;
        }

        public async Task<IController> GetById(Guid id) => await _context.Controllers.FindAsync(id);

        public async Task ReParentEnvironment([NotNull] IController controller, [NotNull] IEnvironment environment)
        {
            // convert to concrete entity types.
            var concreteController = controller as Controller;
            var concreteEnvironment = environment as Environment;

            if (concreteController == null || concreteEnvironment == null)
                throw new ArgumentException("Mismatch infrastructure types");
            
            // ensure navigations are loaded:
            var collection = _context.Entry(concreteController).Collection(c => c.BackedEnvironments);
            if (!collection.IsLoaded) await collection.LoadAsync();

            var reference = _context.Entry(concreteEnvironment)
                .Reference(e => e.RealController);
            if (!reference.IsLoaded) await reference.LoadAsync();
            
            // TODO: research if EF will automatically 'remove' the environment from its existing 
            //       controller's collection on save.

            concreteEnvironment.RealController = concreteController;
            concreteController.Environments.Add(concreteEnvironment);

            await _context.SaveChangesAsync();
        }
    }
}