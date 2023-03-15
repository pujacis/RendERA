using RendERA.DB.Models;
using RendERA.DB.ViewModels;
using RendERA.ServiceManager.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RendERA.ServiceManager.Services
{
    public class ProjectSrv : IProjectSrv
    {
        private readonly RendERA.Infrastructure.IRepositories.IUnitOfWork _unitOfWork;
        public ProjectSrv(Infrastructure.IRepositories.IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        public void Delete(int id)
        {
            if (GetById(id) != null)
            {
                _unitOfWork.IProjectRepo.Remove(id);
                _unitOfWork.IProjectRepo.Save();
            }
        }

        public List<ProjectVM> GetAll()
        {
            var query = from t in _unitOfWork.IProjectRepo.Table.OrderByDescending(a => a.Name)
                        select new ProjectVM
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Status = t.Status,
                            PartnerId = t.PartnerId,
                            CreatedDate = t.CreatedDate,
                            ModifiedDate = t.ModifiedDate,
                            PartnerName = _unitOfWork.IUsersRepo.Table.Where(a => a.UserId == t.PartnerId).FirstOrDefault().UserName

                        };
            if (query != null)
            {
                return query.ToList();
            }
            return null;
        }
        
        public List<ProjectVM> GetProjectListByPartnerId(Guid id)
        {
            var query = from t in _unitOfWork.IProjectRepo.Table.Where(a=> a.PartnerId == id).OrderByDescending(a => a.CreatedDate)
                        select new ProjectVM
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Status = t.Status,
                            PartnerId = t.PartnerId,
                            CreatedDate = t.CreatedDate,
                            ModifiedDate = t.ModifiedDate,
                            PartnerName = _unitOfWork.IUsersRepo.Table.Where(a => a.UserId == t.PartnerId).FirstOrDefault().UserName

                        };
            if (query != null)
            {
                return query.ToList();
            }
            return null;
        }

        public ProjectVM GetById(int id)
        {
            var model = _unitOfWork.IProjectRepo.Table.Where(a => a.Id == id).FirstOrDefault();
            if (model != null)
            {
                var vm = new ProjectVM()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Status = model.Status,
                    PartnerId = model.PartnerId,
                    CreatedDate = model.CreatedDate,
                    PartnerName = _unitOfWork.IUsersRepo.Table.Where(a => a.UserId == model.PartnerId).FirstOrDefault().UserName
                };
                return vm;
            }
            return null;
        }

        public void Insert(ProjectVM model)
        {
            if (model != null)
            {
                var m = new Project()
                {
                    Name = model.Name,
                    Status = model.Status,
                    PartnerId = model.PartnerId,
                    CreatedDate = DateTime.Now
                };
                _unitOfWork.IProjectRepo.Add(m);
                _unitOfWork.IProjectRepo.Save();
            }
        }

        public void Update(ProjectVM model)
        {
            if (model != null)
            {
                var m = _unitOfWork.IProjectRepo.Table.Where(a => a.Id == model.Id).FirstOrDefault();
                m.Name = model.Name;
                m.Status = model.Status;
                m.PartnerId = model.PartnerId;
                m.ModifiedDate = DateTime.Now;
                _unitOfWork.IProjectRepo.Update(m);
                _unitOfWork.IProjectRepo.Save();
            }
        }
    }
}
