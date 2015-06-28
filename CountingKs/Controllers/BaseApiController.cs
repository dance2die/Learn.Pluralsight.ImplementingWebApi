using System.Web.Http;
using CountingKs.Data;
using CountingKs.Models;

namespace CountingKs.Controllers
{
	public abstract class BaseApiController : ApiController
	{
		private readonly ICountingKsRepository _repo;
		private ModelFactory _modelFactory;

		public BaseApiController(ICountingKsRepository repo)
		{
			_repo = repo;
		}

		protected ICountingKsRepository TheRepository
		{
			get { return _repo; }
		}

		protected ModelFactory TheModelFactory
		{
			get
			{
				if (_modelFactory == null)
					_modelFactory = new ModelFactory(Request, TheRepository);
				return _modelFactory;
			}
		}
	}
}
