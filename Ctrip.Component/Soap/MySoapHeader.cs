using System.Web.Services.Protocols;

namespace Ctrip.Component
{
    /// <summary>
    /// [SoapHeader("header", Direction = SoapHeaderDirection.In)]
    /// </summary>
    public class MySoapHeader : SoapHeader
    {
        string _name;
        string _passWord;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string PassWord
        {
            get { return _passWord; }
            set { _passWord = value; }
        }
    }
}
