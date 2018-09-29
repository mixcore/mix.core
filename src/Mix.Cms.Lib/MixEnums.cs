using System;
using System.Collections.Generic;
using System.Text;

namespace Mix.Cms.Lib
{
    public class MixEnums
    {
        #region Common
        public enum CatePosition
        {
            Nav = 1,
            Top = 2,
            Left = 3,
            Footer = 4
        }


        public enum ConfigurationCategory
        {
            PageSize,
            Site,
            Email
        }


        public enum FileFolder
        {
            Styles,
            Scripts,
            Images,
            Fonts,
            Others
        }



        public enum EnumTemplateFolder
        {
            Layouts,
            Pages,
            Modules,
            Articles,
            Products,
            Widgets,
            Masters,
        }
        #endregion

        #region Status

        public enum PageStatus
        {
            Deleted = 0,
            Preview = 1,
            Published = 2,
            Draft = 3,
            Schedule = 4
        }

        public enum MixContentStatus
        {
            Deleted = 0,
            Preview = 1,
            Published = 2,
            Draft = 3,
            Schedule = 4
        }

        public enum MixUserStatus
        {
            Deleted = 0,
            Actived = 1,
            Banned = 3
        }

        #endregion

        #region Types
        public enum MixPageType
        {
            Blank = 0,
            Article = 1,
            List = 2,
            Home = 3,
            StaticUrl = 4,
            Modules = 5,
            ListProduct = 6
        }

        public enum DataType
        {
            String = 0,
            Int = 1,
            Image = 2,
            Icon = 3,
            CodeEditor = 4,
            Html = 5,
            TextArea = 6,
            Boolean = 7,
            MdTextArea = 8
        }

        public enum ModuleType
        {
            Root,
            SubPage,
            SubArticle,
            SubProduct,
            Form
        }

        public enum UrlAliasType
        {
            Page,
            Article,
            Product,
            Module,
            ModuleData
        }

        public enum SearchType
        {
            All,
            Article,
            Module,
            Page
        }
        #endregion

        public enum ResponseKey
        {
            NotFound = 0,
            OK = 1,
            BadRequest = 2
        }
    }
}
