using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Grpc
{
    public class CrudService : Crud.CrudBase
    {
        private readonly List<Model> _models = new List<Model>();
        private int idCount = 0;
        private readonly ILogger<GreeterService> _logger;
        public CrudService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<ModelList> GetAll(Empty empty, ServerCallContext context)
        {
            ModelList pl = new ModelList();
            //pl.Models.AddRange(_models);
            return Task.FromResult(pl);
        }
        //public override Task<Model> Get(Id productId, ServerCallContext context)
        //{
        //    return Task.FromResult( //
        //        (from p in _models where p.Id == productId select p).FirstOrDefault());
        //}
        //public override Task<Model> Insert(Model product, ServerCallContext context)
        //{
        //    product.Id = idCount++;
        //    _models.Add(product);
        //    return Task.FromResult(product);
        //}
        //public override Task<Model> Update(Model product, ServerCallContext context)
        //{
        //    var productToUpdate = (from p in _models where p.Id == product.Id select p).FirstOrDefault();
        //    if (productToUpdate != null)
        //    {
        //        productToUpdate.Name = product.Name;
        //        productToUpdate.Amount = product.Amount;
        //        productToUpdate.Brand = product.Brand;
        //        productToUpdate.Value = product.Value;
        //        return Task.FromResult(product);
        //    }
        //    return Task.FromException<Model>(new EntryPointNotFoundException());
        //}
        //public override Task<Empty> Delete(Id productId, ServerCallContext context)
        //{
        //    var productToDelete = (from p in _models where p.Id == productId.Id select p).FirstOrDefault();
        //    if (productToDelete == null)
        //    {
        //        return Task.FromException<Empty>(new EntryPointNotFoundException());
        //    }
        //    _models.Remove(productToDelete);
        //    return Task.FromResult(new Empty());
        //}
    }
}
