﻿using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Generated;
using System;
using UnityEngine.Events;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace Trinity
{
    public static class ILRuntimeUtility
    {
        public static void InitILRuntime(AppDomain appDomain)
        {
            //TODO:注册重定向方法

            //TODO:适配委托
            appDomain.DelegateManager.RegisterMethodDelegate<float>();
            appDomain.DelegateManager.RegisterMethodDelegate<object, GameFramework.Event.GameEventArgs>();

            //TODO:注册委托
            appDomain.DelegateManager.RegisterDelegateConvertor<UnityAction>((action) =>
            {
                return new UnityAction(() =>
                {
                    ((Action)action)();
                });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<UnityAction<float>>((action) =>
            {
                return new UnityAction<float>((a) =>
                {
                    ((Action<float>)action)(a);
                });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<System.EventHandler<GameFramework.Event.GameEventArgs>>((act) =>
            {
                return new System.EventHandler<GameFramework.Event.GameEventArgs>((sender, e) =>
                {
                    ((Action<System.Object, GameFramework.Event.GameEventArgs>)act)(sender, e);
                });
            });


            //注册CLR绑定代码
            CLRBindings.Initialize(appDomain);

            //TODO:注册适配器
            appDomain.RegisterCrossBindingAdaptor(new GameEventArgsAdaptor());
            appDomain.RegisterCrossBindingAdaptor(new IReferenceAdaptor());

            //注册LitJson
            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appDomain);
        }
    }
}

