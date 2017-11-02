/*
 *   create file time 2017/11/02
 *   signal base file.
 * */

using System;
using System.Collections.Generic;

namespace Framework.Engine
{
    public interface IBaseSignal
    {
        /// <summary>
        /// 调度信号
        /// </summary>
        /// <param name="args"></param>
        void Dispath(object[] args);

        /// <summary>
        /// 添加信号
        /// </summary>
        /// <param name="callback"></param>
        void AddListener(Action<IBaseSignal, object[]> callback);

        /// <summary>
        /// 添加一次信号
        /// </summary>
        /// <param name="callback"></param>
        void AddOnce(Action<IBaseSignal, object[]> callback);

        /// <summary>
        /// 移除信号
        /// </summary>
        /// <param name="callback"></param>
        void RemoveListener(Action<IBaseSignal, object[]> callback);

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <returns></returns>
        List<Type> GetTypes();
    }
}