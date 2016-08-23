using System;
using System.Web.Services.Protocols;

namespace Ctrip.Component
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MyExtensionAttribute : SoapExtensionAttribute
    {
        int _priority = 1;

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public override Type ExtensionType
        {
            get { return typeof(MyExtension); }
        }
    }
}
