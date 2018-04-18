using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medit.Framework.Utilities
{
    public class FileUtility
    {
        /// <summary>
        /// 判断文件是否被占用
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static bool IsFileInUse(string filePath)
        {
            bool result = true;
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                result = false;
            }
            catch { }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
            return result;
        }
    }
}
