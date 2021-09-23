using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalksDetailViewModel
    {
        public Walker Walker { get; set; }

        public List<Walks> Walks { get
          
            ; set; } 

        public double TotalDuration { get; set; }


        public Dog Dog { get; set; }

        public Owner Owner { get; set; }

    }


}

