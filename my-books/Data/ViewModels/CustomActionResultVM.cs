using my_books.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Data.ViewModels {
    public class CustomActionResultVM {
        public Exception Exception { get; set; }
        //general properties
        //public object Data { get; set; }
        //specific Properties
        public Publisher Publisher { get; set; }

    }
}
