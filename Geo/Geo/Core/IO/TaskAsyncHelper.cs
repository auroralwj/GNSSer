//2014.10.29, czs, create in numu, 包含一个Stream属性，用于输入输出。

using System;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.Linq;
using System.IO; 
using Geo.IO;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System.Text; 

namespace Geo.IO
{
    public static class TaskAsyncHelper
    {
        ///// <summary>
        ///// 将一个方法function异步运行，在执行完毕时执行回调callback
        ///// </summary>
        ///// <param name="function">异步方法，该方法没有参数，返回类型必须是void</param>
        ///// <param name="callback">异步方法执行完毕时执行的回调方法，该方法没有参数，返回类型必须是void</param>
        //public static async void RunAsync(Action function, Action callback)
        //{
        //    Func<System.Threading.Tasks.Task> taskFunc = () =>
        //    {
        //        return System.Threading.Tasks.Task.Run(() =>
        //        {
        //            function();
        //        });
        //    };
        //    await taskFunc();
        //    if (callback != null)
        //        callback();
        //}

        ///// <summary>
        ///// 将一个方法function异步运行，在执行完毕时执行回调callback
        ///// </summary>
        ///// <typeparam name="TResult">异步方法的返回类型</typeparam>
        ///// <param name="function">异步方法，该方法没有参数，返回类型必须是TResult</param>
        ///// <param name="callback">异步方法执行完毕时执行的回调方法，该方法参数为TResult，返回类型必须是void</param>
        //public static async void RunAsync<TResult>(Func<TResult> function, Action<TResult> callback)
        //{
        //    Func<System.Threading.Tasks.Task<TResult>> taskFunc = () =>
        //    {
        //        return System.Threading.Tasks.Task.Run(() =>
        //        {
        //            return function();
        //        });
        //    };
        //    TResult rlt = await taskFunc();
        //    if (callback != null)
        //        callback(rlt);
        //}
    }
}
