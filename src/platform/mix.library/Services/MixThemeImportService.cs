using Mix.Database.Entities.Cms;
using Mix.Lib.ViewModels;
using System;
using Mix.Heart.Exceptions;
using System.Threading.Tasks;
using Mix.Lib.Dtos;
using System.Linq;
using Mix.Shared.Enums;
using System.Linq.Expressions;
using Mix.Heart.Extensions;
using Mix.Heart.UnitOfWork;
using System.Collections.Generic;
using Mix.Heart.Entities;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Base;

namespace Mix.Lib.Services
{
    public class MixThemeImportService
    {
        private UnitOfWorkInfo _uow;
        private readonly MixCmsContext _context;
        private SiteDataViewModel _siteData;
        private ImportThemeDto _dto;

        #region Dictionaries

        private Dictionary<int, int> dicAliasIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicConfigurationIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicConfigurationContentIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicLanguageIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicLanguageContentIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicModuleIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicModuleContentIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicModuleDataIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicPostIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicPostContentIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicPageIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicPageContentIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicMixDatabaseIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicColumnIds = new Dictionary<int, int>();

        #endregion

        public MixThemeImportService(MixCmsContext context)
        {
            _context = context;
            _uow = new UnitOfWorkInfo(_context);
            
        }

        #region Import

        public async Task<SiteDataViewModel> ImportSelectedItemsAsync(ImportThemeDto request)
        {
            try
            {
                _uow.Begin();
                _dto = request;
                _siteData = new(_dto);

                // Import Configurations
                ImportData(_siteData.Configurations, dicConfigurationIds);
                ImportContentData(_siteData.ConfigurationContents, dicConfigurationContentIds, dicConfigurationIds);
                
                // Import Languages
                ImportData(_siteData.Languages, dicLanguageIds);
                ImportContentData(_siteData.LanguageContents, dicLanguageContentIds, dicLanguageIds);

                // Import Posts
                ImportData(_siteData.Posts, dicPostIds);
                ImportContentData(_siteData.PostContents, dicPostContentIds, dicPostIds);

                // Import Pages
                ImportData(_siteData.Pages, dicPageIds);
                ImportPageContents();

                // Import Modules
                ImportData(_siteData.Modules, dicModuleIds);
                ImportModuleContents();
                ImportContentData(_siteData.ModuleDatas, dicModuleDataIds, dicModuleIds);

                // Import Databases
                ImportDatabases();
                ImportDatabaseColumns();


                ImportAssociationData(_siteData.PageModules, dicPageIds, dicModuleIds);
                ImportAssociationData(_siteData.PagePosts, dicPageIds, dicPostIds);
                ImportAssociationData(_siteData.ModulePosts, dicModuleIds, dicPostIds);

                ImportDatabaseData();

                ImportData(_siteData.MixUrlAliases, dicAliasIds);

                await _context.SaveChangesAsync();
                await _uow.CompleteAsync();

                return _siteData;
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }
        }

        
        #region Import Page Data

        private void ImportPageContents()
        {
            var startId = _context.MixPageContent.Max(m => m.Id);
            foreach (var item in _siteData.PageContents)
            {
                startId++;
                dicPageContentIds.Add(item.Id, startId);

                while (_context.MixPageContent.Any(m => m.SeoName == item.SeoName))
                {
                    item.SeoName = $"{item.SeoName}-1";
                }

                item.Id = startId;
                item.ParentId = dicPageIds[item.ParentId];
                _context.MixPageContent.Add(item);
            }

        }
        #endregion Import Page

        #region Import Module Data

        private void ImportModuleContents()
        {
            var startId = _context.MixModuleContent.Max(m => m.Id);
            foreach (var item in _siteData.ModuleContents)
            {
                if(!_context.MixModuleContent.Any(m => m.SystemName == item.SystemName))
                {
                    startId++;
                    dicModuleContentIds.Add(item.Id, startId);
                    item.Id = startId;
                    item.ParentId = dicModuleIds[item.ParentId];
                    _context.MixModuleContent.Add(item);
                }
            }

        }


        #endregion Import Module

        #region Import Database Data

        private void ImportDatabases()
        {
            var startId = _context.MixDatabase.Max(m => m.Id);
            foreach (var item in _siteData.Databases)
            {
                startId++;
                dicMixDatabaseIds.Add(item.Id, startId);

                while (_context.MixDatabase.Any(m => m.SystemName == item.SystemName))
                {
                    item.SystemName = $"{item.SystemName}_1";
                }

                item.Id = startId;
                _context.MixDatabase.Add(item);
            }

        }

        private void ImportDatabaseColumns()
        {
            var startId = _context.MixDatabaseColumn.Max(m => m.Id);
            foreach (var item in _siteData.DatabaseColumns)
            {
                startId++;
                dicColumnIds.Add(item.Id, startId);
                item.Id = startId;
                item.MixDatabaseId = dicMixDatabaseIds[item.MixDatabaseId];
                item.MixDatabaseName = _siteData.Databases.First(m => m.Id == item.MixDatabaseId).SystemName;
                _context.MixDatabaseColumn.Add(item);
            }

        }

        private void ImportDatabaseData()
        {
            ImportGuidDatas(_siteData.Datas);
            ImportGuidDatas(_siteData.DataContents);
            ImportGuidDatas(_siteData.DataContentValues);
            ImportGuidDatas(_siteData.DataContentAssociations);
        }

        #endregion Import Module

        private void ImportGuidDatas<T>(List<T> data)
            where T : EntityBase<Guid>
        {
            foreach (var item in data)
            {
                _context.Entry(item).State = EntityState.Added;
            }
        }

        private void ImportData<T>(List<T> data, Dictionary<int, int> dic)
            where T: EntityBase<int>
        {
            var startId = _context.Set<T>().Max(m => m.Id);
            foreach (var item in data)
            {
                dic.Add(item.Id, startId + 1);
                item.Id = startId + 1;
                _context.Entry(item).State = EntityState.Added;
            }
        }
        
        private void ImportContentData<T>(List<T> data, Dictionary<int, int> dic, Dictionary<int, int> parentDic)
            where T: MultilanguageContentBase<int>
        {
            var startId = _context.Set<T>().Max(m => m.Id);
            foreach (var item in data)
            {
                dic.Add(item.Id, startId + 1);
                item.Id = startId + 1;
                item.ParentId = parentDic[item.ParentId];
                _context.Entry(item).State = EntityState.Added;
            }
        }
        
        private void ImportAssociationData<T>(
            List<T> data,
            Dictionary<int, int> leftDic,
            Dictionary<int, int> rightDic)
            where T: AssociationBase<int>
        {
            var startId = _context.Set<T>().Max(m => m.Id);
            foreach (var item in data)
            {
                item.Id = startId + 1;
                item.LeftId = leftDic[item.LeftId];
                item.RightId = rightDic[item.RightId];
                _context.Entry(item).State = EntityState.Added;
            }
        }
        #endregion Import
    }
}
