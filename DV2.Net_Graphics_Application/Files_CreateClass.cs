using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DV2.Net_Graphics_Application
{
    public partial class Files_CreateClass
    {
        public void File_ex()
        {
            try
            {

            }

            catch (SystemException sye)
            {
                System.Console.WriteLine("ERROR Code 999");
                System.Console.WriteLine("The ERROR comes from Files_CreateClass.cs");
                System.Console.WriteLine(sye.Message);
            }
        }
    }
}
