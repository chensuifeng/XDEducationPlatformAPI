using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace XDEducationPlatformAPI.UrlDecrypt
{
    public static class RegexExtends
    {
        /// <summary>
        /// 返回指定 匹配项
        /// <para>默认返点 索引1内容</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static string Match(this string source, string regex, int index = 1)
        {
            return Regex.Match(source, regex).Groups[index].Value;
        }
    }
}