using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Organizations.ArinaOrganization
{
    public static class Mylar // Emerald Shrine
    {
        private static ArCultureInfo _ArinaCultureInfo;
        public static ArCultureInfo ArinaCultureInfo
            => _ArinaCultureInfo ??= new ArCultureInfo();
    }
}
