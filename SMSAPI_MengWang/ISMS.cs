using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.SMSAPI_MengWang
{
    public interface ISMS
    {
        string execute(MWMessage message, int sendType, string IpAndPort, int authenticationMode, bool bKeepAlive);
    }
}