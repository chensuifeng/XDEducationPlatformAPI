using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace XDEducationPlatformAPI.MD5API
{
    public class MD5Decrypt :  IDisposable
    {
        //是否回收完毕
        public bool _alreadyDisposed = false;

        /// <summary>
        /// 解密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ciphertext">密文</param>
        /// <param name="key">解密 key</param>
        /// <returns></returns>
        public T DecryptByKey<T>(string ciphertext,string key) where T : new()
        {

            T tt = new T() { };
            return tt;
        }

        public  void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //这里的参数表示示是否需要释放那些实现IDisposable接口的托管对象
        protected virtual void Dispose(bool shouldDisposeManagedReources)
        {
            if (_alreadyDisposed)
            {
                return;  //如果已经被回收，就中断执行
            }
            if (shouldDisposeManagedReources)
            {
                //TODO:释放那些实现IDisposable接口的托管对象
            }
            //TODO:释放非托管资源，设置对象为null
            _alreadyDisposed = true;
        }

        /// <summary>
        /// 析构函数
        /// 当托管堆上的对象没有被其它对象引用，GC会在回收对象之前，调用对象的析构函数。这里的~MD5Decrypt()析构函数的意义在于告诉GC你可以回收我，Dispose(false)表示在GC回收的时候，就不需要手动回收了。
        /// </summary>
        ~MD5Decrypt()
        {
            Dispose(false);
        }

        public void MethodForPublic()
        {
            if (_alreadyDisposed)
                throw new Exception("object has been disposed!");
            // do the normal things
        }
}
}