using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;

namespace DogGo.Repositories
{
    public interface IDogRepository
    {
        List<Dog> GetAllDogs();

        Dog GetDogById(int id);

        void AddDog(Dog dog);

        //void EditDog(Dog dog);

        //void DeleteDog(int id);


    }
}
