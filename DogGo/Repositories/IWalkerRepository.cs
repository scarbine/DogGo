using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using DogGo.Models;

namespace DogGo.Repositories
{
        public interface IWalkerRepository
        {
            List<Walker> GetAllWalkers();
            Walker GetWalkerById(int id);
        }

   
}
