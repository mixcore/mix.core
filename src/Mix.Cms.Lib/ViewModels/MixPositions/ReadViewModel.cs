using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixPositions
{
    public class ReadViewModel
        : ViewModelBase<MixCmsContext, MixPosition, ReadViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public MixEnums.MixContentStatus Status { get; set; }

        #endregion Models

        #endregion Properties

        #region Contructors

        public ReadViewModel() : base()
        {
        }

        public ReadViewModel(MixPosition model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Expands

        public static async Task<RepositoryResponse<bool>> ImportPositions(List<MixPosition> arrPosition,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            bool isRoot = _context == null;
            var context = _context ?? new MixCmsContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();

            try
            {
                foreach (var item in arrPosition)
                {
                    context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
                await context.SaveChangesAsync();
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<MixPosition>(ex, isRoot, transaction);
                result.IsSucceed = false;
                result.Errors = error.Errors;
                result.Exception = error.Exception;
            }
            finally
            {
                //if current Context is Root
                if (isRoot)
                {
                    context?.Dispose();
                }
            }
            return result;
        }

        #endregion Expands
    }
}