using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Medit.Framework.Utilities
{
    /// <summary>
    /// 缓存工具类
    /// </summary>
    public class CacheUtility
    {
        private static Cache _cache = HttpRuntime.Cache;

        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="key">键</param>
        public static object Get(string key)
        {
            return _cache[key];
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set(string key, object value)
        {
            _cache.Insert(key, value);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="dep">依赖文件</param>
        public static void SetCache(string key, object value, CacheDependency dep)
        {
            _cache.Insert(key, value, dep,
                            Cache.NoAbsoluteExpiration, //从不过期
                            Cache.NoSlidingExpiration, //禁用可调过期
                            CacheItemPriority.NotRemovable,
                            null);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="filename">依赖文件名</param>
        public static void SetCache(string key, object value, string filename)
        {
            CacheDependency dep = new CacheDependency(filename);
            _cache.Insert(key, value, dep,
                            Cache.NoAbsoluteExpiration, //从不过期
                            Cache.NoSlidingExpiration, //禁用可调过期
                            CacheItemPriority.NotRemovable,
                            null);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="timeout">过期时间</param>
        public static void SetCache(string key, object value, TimeSpan timeout)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            _cache.Insert(key, value, null, DateTime.MaxValue, timeout, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveAllCache(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            IDictionaryEnumerator enumerator = _cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                _cache.Remove(enumerator.Key.ToString());
            }
        }
    }
}
