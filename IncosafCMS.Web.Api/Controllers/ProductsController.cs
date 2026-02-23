using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IncosafCMS.WebApi.Controllers
{
    public class ProductsController : ApiController
    {
        IService<Product> service;
        IUnitOfWork uow;
        public ProductsController(IService<Product> _service, IUnitOfWork _uow)
        {
            service = _service;
            uow = _uow;
        }
        // GET api/<controller>
        public IEnumerable<Product> Get()
        {
            return service.GetAll();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}