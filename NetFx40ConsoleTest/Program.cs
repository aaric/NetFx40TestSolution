namespace NetFx40ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //WaitForMultipleObjectsTest.Main1(null);
            //NLuaTest.Main1(null);
            //M2MqttTest.Main1(null);
            //MQTTnet40Test.Main1(null);
            //XPath2Test.Main1(null);
            HttpClientTest.Main1(null).Wait();
        }
    }
}