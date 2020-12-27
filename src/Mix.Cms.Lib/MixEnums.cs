using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib
{
    public class MixEnums
    {
        #region Common

        public enum DatabaseProvider
        {
            MSSQL,
            MySQL,
            PostgreSQL
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

        #endregion Common

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
            Deleted,
            Preview,
            Published,
            Draft,
            Schedule
        }

        public enum ResponseStatus
        {
            Ok = 200,
            BadRequest = 400,
            UnAuthorized = 401,
            Forbidden = 403,
            ServerError = 500
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
            Deleted,
            Actived,
            Banned
        }

        #endregion Status

        #region Types

        public enum MixAddictionalType
        {
            Page,
            Post,
            Module
        }

        public enum MixPageType
        {
            Article = 1,
            ListPost = 2,
            Home = 3,
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

        //public enum MixAttributeSetType
        //{
        //    System = 0,
        //    Page = 1,
        //    Post = 2,
        //    Module = 3,
        //    Service = 4
        //}

        public enum MixAttributeSetDataType
        {
            System = 0,
            Set = 1,
            Post = 2,
            Page = 3,
            Module = 4,
            Service = 5
        }

        public enum MixRelatedType
        {
            PageToPage = 0,
            PageToPost = 1,
            PageToModule = 2,
            PageToData = 3,
            ModuleToPost = 4,
            ModuleToPage = 5,
            ModuleToData = 6,
            PostToData = 7,
            DataToData = 8
        }

        public enum MixDataType
        {
            Custom,

            //
            // Summary:
            //     Represents an instant in time, expressed as a date and time of day.
            DateTime,

            //
            // Summary:
            //     Represents a date value.
            Date,

            //
            // Summary:
            //     Represents a time value.
            Time,

            //
            // Summary:
            //     Represents a continuous time during which an object exists.
            Duration,

            //
            // Summary:
            //     Represents a phone number value.
            PhoneNumber,

            //
            // Summary:
            //     Represents a currency value.
            Double,

            //
            // Summary:
            //     Represents text that is displayed.
            Text,

            //
            // Summary:
            //     Represents an HTML file.
            Html,

            //
            // Summary:
            //     Represents multi-line text.
            MultilineText,

            //
            // Summary:
            //     Represents an email address.
            EmailAddress,

            //
            // Summary:
            //     Represent a password value.
            Password,

            //
            // Summary:
            //     Represents a URL value.
            Url,

            //
            // Summary:
            //     Represents a URL to an image.
            ImageUrl,

            //
            // Summary:
            //     Represents a credit card number.
            CreditCard,

            //
            // Summary:
            //     Represents a postal code.
            PostalCode,

            //
            // Summary:
            //     Represents file upload data type.
            Upload,

            Color,
            Boolean,
            Icon,
            VideoYoutube,
            TuiEditor,
            Integer,
            Reference,
            QRCode
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
            Post,
            AttributeSet
        }

        #endregion Types

        public enum ResponseKey
        {
            NotFound = 0,
            OK = 1,
            BadRequest = 2
        }

        public enum CompareType
        {
            Eq = 1,
            Lt = 2,
            Gt = 3,
            Lte = 4,
            Gte = 5,
            In = 6
        }

        public static List<object> EnumToObject(Type enumType)
        {
            List<object> result = new List<object>();
            var values = Enum.GetValues(enumType);
            foreach (var item in values)
            {
                result.Add(new { name = Enum.GetName(enumType, item), value = Enum.GetName(enumType, item) });
            }
            return result;
        }
    }
}