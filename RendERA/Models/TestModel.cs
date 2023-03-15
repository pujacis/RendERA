
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RendERA.Models
{
    public class TestModel
    {
       
     //   List<tmodel> dropdown { get; set; }
      public  List<input> inputfiled { get; set; }
      public  List<bindmodel> dropfiledline { get; set; }
    }

    public class bindmodel {
        public string name { get; set; }
        public List<tmodel> dropdown { get; set; }
        public int Seletedvalue { get; set; }

    }


    public class tmodel {
        public int Id { get; set; }
        public string name { get; set; }
    }

    public class input
    {
        public int Id { get;set; }
        public string name { get; set; }
        public string value { get; set; }
    }
}
