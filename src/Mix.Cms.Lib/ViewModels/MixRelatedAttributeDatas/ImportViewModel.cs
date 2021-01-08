﻿using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas
{
    public class ImportViewModel
        : ViewModelBase<MixCmsContext, MixDatabaseContentAssociation, ImportViewModel>
    {
        #region Properties

        #region Model

        /*
         * Attribute Set Data Id
         */

        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
        [JsonProperty("dataId")]
        public string DataId { get; set; }
        [JsonProperty("cultures")]
        public List<Domain.Core.Models.SupportedCulture> Cultures { get; set; }
        /*
         * Parent Id: PostId / PageId / Module Id / Data Id / Attr Set Id
         */

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("parentType")]
        public MixDatabaseContentAssociationType ParentType { get; set; }

        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("attributeSetName")]
        public string AttributeSetName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }
        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }
        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }
        [JsonProperty("priority")]
        public int Priority { get; set; }
        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }
        #endregion Model

        #region Views

        [JsonProperty("isProcessed")]
        public bool IsProcessed { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportViewModel() : base()
        {
        }

        public ImportViewModel(MixDatabaseContentAssociation model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            //var getPost = MixAttributeSetDatas.ReadViewModel.Repository.GetSingleModel(
            //    m => m.Id == Id && m.Specificulture == Specificulture
            //    , _context: _context, _transaction: _transaction);
            //if (getPost.IsSucceed)
            //{
            //    this.RelatedAttributeData = getPost.Data;
            //}
        }

        #endregion Overrides
    }
}