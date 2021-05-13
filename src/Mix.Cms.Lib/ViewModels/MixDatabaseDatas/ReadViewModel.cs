using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixDatabaseDatas
{
    public class ReadViewModel
      : ViewModelBase<MixCmsContext, MixDatabaseData, ReadViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("cultures")]
        public List<SupportedCulture> Cultures { get; set; }

        [JsonProperty("mixDatabaseId")]
        public int MixDatabaseId { get; set; }

        [JsonProperty("mixDatabaseName")]
        public string MixDatabaseName { get; set; }

        [JsonProperty("createdBy")]
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

        #endregion Models

        #region Views

        [JsonProperty("values")]
        public List<MixDatabaseDataValues.ReadViewModel> Values { get; set; }

        [JsonProperty("columns")]
        public List<MixDatabaseColumns.ReadViewModel> Columns { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadViewModel() : base()
        {
        }

        public ReadViewModel(MixDatabaseData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getValues = MixDatabaseDataValues.ReadViewModel
                .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction);
            if (getValues.IsSucceed)
            {
                Values = getValues.Data.OrderBy(a => a.Priority).ToList();
            }
            else
            {
                Console.WriteLine(getValues.Exception);
            }

            Columns = MixDatabaseColumns.ReadViewModel.Repository.GetModelListBy(f => f.MixDatabaseId == MixDatabaseId, _context, _transaction).Data;
            foreach (var field in Columns.OrderBy(f => f.Priority))
            {
                var val = Values.FirstOrDefault(v => v.MixDatabaseColumnId == field.Id);
                if (val == null)
                {
                    val = new MixDatabaseDataValues.ReadViewModel(
                        new MixDatabaseDataValue() { MixDatabaseColumnId = field.Id }
                        , _context, _transaction)
                    {
                        Column = field,
                        MixDatabaseColumnName = field.Name,
                        StringValue = field.DefaultValue,
                        Priority = field.Priority
                    };
                    Values.Add(val);
                }
                val.MixDatabaseName = MixDatabaseName;
                val.Priority = field.Priority;
                val.Column = field;
            };
        }

        #endregion Overrides
    }
}