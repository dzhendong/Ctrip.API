using System;
using System.Web.Services.Protocols;

namespace Ctrip.Component
{
    /// <summary>
    /// [MyExtensionAttribute]
    /// [SoapHeader("header", Direction = SoapHeaderDirection.In)]
    /// </summary>
    public class MyExtension : SoapExtension
    {
        /// <summary>
        /// 这个override的方法会被调用四次
        /// 分别是SoapMessageStage的BeforeSerialize,AfterSerialize,BeforeDeserialize,AfterDeserialize
        /// </summary>
        /// <param name="message"></param>
        public override void ProcessMessage(SoapMessage message)
        {
            if (message.Stage == SoapMessageStage.AfterDeserialize)//反序列化之后处理
            {
                bool check = false;
                foreach (SoapHeader header in message.Headers)
                {
                    if (header is MySoapHeader)
                    {
                        MySoapHeader myHeader = (MySoapHeader)header;

                        //解密
                        //myHeader.Name = Security.Decrypt(myHeader.Name);
                        //myHeader.PassWord = Security.Decrypt(myHeader.PassWord);

                        if (myHeader.Name == "admin" || myHeader.PassWord == "admin")
                        {
                            check = true;
                            break;
                        }
                    }
                }
                if (!check)
                {
                    throw new SoapHeaderException("认证失败", SoapException.ClientFaultCode);
                }
            }
        }

        public override Object GetInitializer(Type type)
        {
            return GetType();
        }

        public override Object GetInitializer(LogicalMethodInfo info, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override void Initialize(Object initializer)
        {
        }
    }
}
