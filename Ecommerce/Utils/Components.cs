using Ecommerce.DAL;
using Ecommerce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Utils
{
    public static class Components
    {
        public static IDataLayer DataLayer = new TestDataLayer();
    }
}