#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class GameEngineIUIModelControlWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Game.Engine.IUIModelControl);
			Utils.BeginObjectRegister(type, L, translator, 0, 10, 5, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsGoBack", _m_IsGoBack);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsClose", _m_IsClose);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLinksUI", _m_GetLinksUI);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCloseOther", _m_GetCloseOther);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitUIData", _m_InitUIData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OpenSelf", _m_OpenSelf);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseSelf", _m_CloseSelf);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLayerTop", _m_SetLayerTop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetUIDisable", _m_SetUIDisable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "ControlTarget", _g_get_ControlTarget);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Layer", _g_get_Layer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_ModelObjectPath", _g_get_m_ModelObjectPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_IsOnlyOne", _g_get_m_IsOnlyOne);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_IsOnlyID", _g_get_m_IsOnlyID);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_ModelObjectPath", _s_set_m_ModelObjectPath);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_IsOnlyOne", _s_set_m_IsOnlyOne);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_IsOnlyID", _s_set_m_IsOnlyID);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Game.Engine.IUIModelControl does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsGoBack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.IsGoBack(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsClose(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.IsClose(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLinksUI(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.Collections.Generic.List<string> gen_ret = gen_to_be_invoked.GetLinksUI(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCloseOther(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.List<string> _others = (System.Collections.Generic.List<string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<string>));
                    
                        bool gen_ret = gen_to_be_invoked.GetCloseOther( ref _others );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _others);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitUIData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Game.Engine.UILayer _layer;translator.Get(L, 2, out _layer);
                    object[] _arms = translator.GetParams<object>(L, 3);
                    
                    gen_to_be_invoked.InitUIData( _layer, _arms );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenSelf(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _target = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.OpenSelf( _target );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseSelf(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _manager = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.CloseSelf( _manager );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.CloseSelf(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.Engine.IUIModelControl.CloseSelf!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLayerTop(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.SetLayerTop(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUIDisable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _show = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetUIDisable( _show );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.Update(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ControlTarget(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ControlTarget);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Layer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Layer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_ModelObjectPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.m_ModelObjectPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_IsOnlyOne(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.m_IsOnlyOne);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_IsOnlyID(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.m_IsOnlyID);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_ModelObjectPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_ModelObjectPath = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_IsOnlyOne(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_IsOnlyOne = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_IsOnlyID(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.Engine.IUIModelControl gen_to_be_invoked = (Game.Engine.IUIModelControl)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_IsOnlyID = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
