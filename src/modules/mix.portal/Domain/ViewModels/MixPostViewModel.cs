﻿using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixPostViewModel : SiteDataWithContentViewModelBase<MixCmsContext, MixPost, int, MixPostContent, MixPostContentViewModel>
    {
        #region Contructors

        public MixPostViewModel()
        {
        }

        public MixPostViewModel(MixPost entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixPostViewModel(Repository<MixCmsContext, MixPost, int> repository) : base(repository)
        {
        }

        public MixPostViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion
    }
}