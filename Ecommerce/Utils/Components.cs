using Ecommerce.DAL;
using Ecommerce.Interfaces;
using System;

namespace Ecommerce.Utils
{
    public static class Components
    {
        public static IDataLayer DataLayer = new SQLServerDataLayer();
        public static Random Random = new Random();
    }
}