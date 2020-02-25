using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSets
{
    public class ReadMvcViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSet, ReadMvcViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ReferenceId")]
        public int? ReferenceId { get; set; }

        [JsonProperty("type")]
        public int? Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("formTemplate")]
        public string FormTemplate { get; set; }

        [JsonProperty("edmTemplate")]
        public string EdmTemplate { get; set; }

        [JsonProperty("edmSubject")]
        public string EdmSubject { get; set; }

        [JsonProperty("edmFrom")]
        public string EdmFrom { get; set; }

        [JsonProperty("edmAutoSend")]
        public bool? EdmAutoSend { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        #endregion Models

        #region Views

        public PaginationModel<MixRelatedAttributeDatas.ReadMvcViewModel> Data { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadMvcViewModel() : base()
        {
        }

        public ReadMvcViewModel(MixAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors



        #region Expand

        public void LoadData(string parentId, MixEnums.MixAttributeSetDataType parentType, string specificulture, int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getData = MixRelatedAttributeDatas.ReadMvcViewModel.Repository
            .GetModelListBy(
                m => m.ParentId == parentId && m.ParentType == (int)parentType && m.Specificulture == specificulture
                , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
                , pageSize, pageIndex
                , _context: _context, _transaction: _transaction);

            Data = getData.Data;
        }

        #endregion Expand
    }
}