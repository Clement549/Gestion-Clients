using System;
using System.Collections.Generic;
using System.Text;

namespace Esilv_BDD
{
    class Variables_Manager
    {
        public static string adminLogin = "SERVER=localhost;PORT=3306;"
                                             + "DATABASE = velomax;"
                                             + "UID=root; PASSWORD=root";

        public static double money;

        public static string export_format = ".Json";
    }
}
