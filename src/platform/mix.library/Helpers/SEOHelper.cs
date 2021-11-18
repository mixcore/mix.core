// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.RegularExpressions;

namespace Mix.Lib.Helpers
{
    /// <summary>
    /// SEO Helper
    /// </summary>
    public static class SeoHelper
    {
        //delete special charaters
        /// <summary>
        /// Deletes the special charaters.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string DeleteSpecialCharaters(string str)
        {
            const string replaceChar = "";
            string[] pattern = { ".", "/", "\\", "&", ":", "%" };

            foreach (string item in pattern)
            {
                str = str.Replace(item, replaceChar);
            }
            return str;
        }

        /// <summary>
        /// Gets the seo string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string GetSEOString(string s, char replaceChar = '-')
        {
            return !string.IsNullOrEmpty(s) ? WhiteSpaceToHyphen(ConvertToUnSign(DeleteSpecialCharaters(s)), replaceChar) : s;
        }

        // Chuyển tiếng việt có dấu thành không dấu

        #region convert tieng viet ko dau

        /// <summary>
        /// Converts to un sign.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string ConvertToUnSign(string text)
        {
            if (text != null)
            {
                for (int i = 33; i < 48; i++)
                {
                    text = text.Replace(((char)i).ToString(), "");
                }

                for (int i = 58; i < 65; i++)
                {
                    text = text.Replace(((char)i).ToString(), "");
                }

                for (int i = 91; i < 97; i++)
                {
                    text = text.Replace(((char)i).ToString(), "");
                }

                for (int i = 123; i < 127; i++)
                {
                    text = text.Replace(((char)i).ToString(), "");
                }
            }
            else
            {
                text = "";
            }

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\p{IsCombiningDiacriticalMarks}+");

            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);

            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        #endregion convert tieng viet ko dau

        //change white-space to hyphen
        /// <summary>
        /// Whites the space to hyphen.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string WhiteSpaceToHyphen(string str, char replaceChar = '-')
        {
            string pattern = " |–";
            MatchCollection matchs = Regex.Matches(str, pattern, RegexOptions.IgnoreCase);
            foreach (Match m in matchs)
            {
                str = str.Replace(m.Value[0], replaceChar);
            }
            replaceChar = '\'';
            pattern = "\"|“|”";
            matchs = Regex.Matches(str, pattern, RegexOptions.IgnoreCase);
            foreach (Match m in matchs)
            {
                str = str.Replace(m.Value[0], replaceChar);
            }
            return str.ToLower();
        }
    }
}