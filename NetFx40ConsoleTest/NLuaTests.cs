using System;
using NLua;
using NLua.Exceptions;

namespace NetFx40ConsoleTest
{
    /// <summary>
    /// http://nlua.org
    ///
    /// <code>dotnet add package NLua --version 1.3.2.1</code>
    /// </summary>
    public class NLuaTests
    {
        public static void Main1(string[] args)
        {
            try
            {
                Lua lua = new Lua();
                lua.DoFile("test.lua");

                // return list
                LuaTable table = lua.DoString("return my_lua_table()")[0] as LuaTable;

                if (null != table)
                {
                    foreach (object item in table.Values)
                    {
                        Console.WriteLine(item);
                    }
                }

                // return number
                string num = lua.DoString(string.Format("return my_lua_sum({0}, {1})", 10.0, 20.0))[0].ToString();
                Console.WriteLine(num);
            }
            catch (LuaException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}