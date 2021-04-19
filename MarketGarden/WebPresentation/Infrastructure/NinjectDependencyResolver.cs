using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Moq;

namespace WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        private void AddBindings()
        {
            // bindings go here....
            //Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            //mockProductRepository.Setup(m => m.Products).Returns(new List<Product>
            //{
            //    new Product { Name="Football", Price = 25.99M },
            //    new Product { Name="Surf Board", Price = 179.99M },
            //    new Product { Name="Running Shoes", Price = 95.88M }
            //});

            //// _kernel.Bind<IProductRepository>().ToConstant(mockProductRepository.Object);
            //_kernel.Bind<IProductRepository>().To<EFProductRepository>();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}