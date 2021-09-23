using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {

        private readonly IWalksRepository _walksRepo;
        private readonly IOwnerRepository _ownerRepo;
        private readonly IDogRepository _dogRepo;
        private readonly IWalkerRepository _walkerRepo;

        public WalkersController(IWalksRepository walksRepository, IOwnerRepository ownerRepository,
         IDogRepository dogRepository,
        IWalkerRepository walkerRepository)
        {
            _walksRepo = walksRepository;
            _ownerRepo = ownerRepository;
            _walkerRepo = walkerRepository;
            _dogRepo = dogRepository;
        }

        // GET: WalkersController
        public ActionResult Index()
        {
            List<Walker> walkers = _walkerRepo.GetAllWalkers();
            return View(walkers);
        }

        // GET: WalkersController/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);

            List<Walks> walks = _walkerRepo.GetAllWalksByWalker(id);

  

            WalksDetailViewModel vm = new WalksDetailViewModel()
            {
                //Owner = new Owner(),
                //Dog = new Dog(),
                Walker = walker,
                Walks = walks
            };
            return View(vm);
        }

        // GET: WalkersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalkersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            _walkerRepo.UpdateWalker(walker);
            return View(walker);
        }

        // POST: WalkersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalkersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
