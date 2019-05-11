using Ecommerce.DAL;
using Ecommerce.Interfaces;

namespace Ecommerce.Utils
{
    public static class Components
    {
        public static IDataLayer DataLayer = new SQLServerDataLayer();
    }
}