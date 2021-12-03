using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;

namespace ChessAPI2.Controllers
{
    public class VersionController : ApiController
    {
        public class Version {
            public string name = "ChessAPI";
            public string version = "0.2";
        }
        public string GetVersion() {
            Version version = new Version();
            return version.ToString();
        }

    }
}
