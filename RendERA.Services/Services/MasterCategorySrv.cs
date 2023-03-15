using RendERA.DB.Models;
using RendERA.DB.ViewModels;
using RendERA.ServiceManager.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RendERA.ServiceManager.Services
{
    public class MasterCategorySrv : IMasterCategorySrv
    {
        private readonly RendERA.Infrastructure.IRepositories.IUnitOfWork _unitOfWork;
        public MasterCategorySrv(Infrastructure.IRepositories.IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        public void Delete(int id)
        {
            if (GetById(id) != null)
            {
                _unitOfWork.IMasterCategoryRepo.Remove(id);
                _unitOfWork.IMasterCategoryRepo.Save();
            }
        }

        public List<MasterCategoryVM> GetAll()
        {
            var query = from t in _unitOfWork.IMasterCategoryRepo.Table.OrderByDescending(a => a.Name)
                        select new MasterCategoryVM
                        {
                            Id = t.Id,
                            Name = t.Name
                        };
            if (query != null)
            {
                return query.ToList();
            }
            return null;
        }
        
        

        public MasterCategoryVM GetById(int id)
        {
            var model = _unitOfWork.IMasterCategoryRepo.Table.Where(a => a.Id == id).FirstOrDefault();
            if (model != null)
            {
                var vm = new MasterCategoryVM()
                {
                    Id = model.Id,
                    Name = model.Name
                };
                return vm;
            }
            return null;
        }

        public void Insert(MasterCategoryVM model)
        {
            if (model != null)
            {
                var m = new MasterCategory()
                {
                    Name = model.Name
                };
                _unitOfWork.IMasterCategoryRepo.Add(m);
                _unitOfWork.IMasterCategoryRepo.Save();
            }
        }

        public void Update(MasterCategoryVM model)
        {
            if (model != null)
            {
                var m = _unitOfWork.IMasterCategoryRepo.Table.Where(a => a.Id == model.Id).FirstOrDefault();
                m.Name = model.Name;
                _unitOfWork.IMasterCategoryRepo.Update(m);
                _unitOfWork.IMasterCategoryRepo.Save();
            }
        }

    }
}
