using System;
using System.Collections.Generic;
using System.Text;

namespace Mix.Cms.Lib
{
    public class MixEnums
    {
        #region Common

        public enum DatabaseProvider
        {
            MSSQL = 1,
            MySQL = 2
        }

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
            Others,
            Templates
        }



        public enum EnumTemplateFolder
        {
            Layouts,
            Pages,
            Modules,
            Forms,
            Edms,
            Posts,
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

        public enum MixOrderStatus
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
            Post = 1,
            ListPost = 2,
            Home = 3,
            StaticUrl = 4,
            Modules = 5,
            ListProduct = 6,
            Gallery = 7,
            System = 8
        }

        public enum MixModuleType
        {
            Content = 0,
            Data = 1,
            ListPost = 2,
            SubPage = 3,
            SubPost = 4,
            SubProduct = 5,
            ListProduct = 6,
            Gallery = 7
        }
        
        public enum MixAttributeSetType
        {
            SubPage = 1,
            SubPost = 2,
            SubProduct = 3,
        } 
        public enum MixAttributeSetDataType
        {
            SubSet = 1,
            SubPost = 2,
            SubPage = 3,
            SubModule= 4
        }

        public enum MixDataType
        {
            Custom = 0,
            //
            // Summary:
            //     Represents an instant in time, expressed as a date and time of day.
            DateTime = 1,
            //
            // Summary:
            //     Represents a date value.
            Date = 2,
            //
            // Summary:
            //     Represents a time value.
            Time = 3,
            //
            // Summary:
            //     Represents a continuous time during which an object exists.
            Duration = 4,
            //
            // Summary:
            //     Represents a phone number value.
            PhoneNumber = 5,
            //
            // Summary:
            //     Represents a currency value.
            Currency = 6,
            //
            // Summary:
            //     Represents text that is displayed.
            Text = 7,
            //
            // Summary:
            //     Represents an HTML file.
            Html = 8,
            //
            // Summary:
            //     Represents multi-line text.
            MultilineText = 9,
            //
            // Summary:
            //     Represents an email address.
            EmailAddress = 10,
            //
            // Summary:
            //     Represent a password value.
            Password = 11,
            //
            // Summary:
            //     Represents a URL value.
            Url = 12,
            //
            // Summary:
            //     Represents a URL to an image.
            ImageUrl = 13,
            //
            // Summary:
            //     Represents a credit card number.
            CreditCard = 14,
            //
            // Summary:
            //     Represents a postal code.
            PostalCode = 15,
            //
            // Summary:
            //     Represents file upload data type.
            Upload = 16,
            Color = 17,
            Boolean = 18,
            Icon = 19,
            VideoYoutube = 20,
            TuiEditor = 21,
            Number = 22,
            Refferrence = 23,
        }

        public enum UrlAliasType
        {
            Page,
            Post,
            Product,
            Module,
            ModuleData
        }

        public enum SearchType
        {
            All,
            Post,
            Module,
            Page
        }

        public enum MixStructureType
        {
            Page,
            Module,
            Post
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
