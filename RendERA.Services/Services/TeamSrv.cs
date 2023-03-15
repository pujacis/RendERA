using RendERA.DB.Models;
using RendERA.DB.ViewModels;
using RendERA.ServiceManager.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RendERA.ServiceManager.Services
{
    public class TeamSrv : ITeamSrv
    {
        private readonly RendERA.Infrastructure.IRepositories.IUnitOfWork _unitOfWork;
        public TeamSrv(Infrastructure.IRepositories.IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        public void Delete(int id)
        {
            if (GetById(id) != null)
            {
                _unitOfWork.ITeamRepo.Remove(id);
                _unitOfWork.ITeamRepo.Save();
            }
        }

        public List<TeamVM> GetAll()
        {
            var query = from t in _unitOfWork.ITeamRepo.Table.OrderByDescending(a => a.Name)
                        select new TeamVM
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Degination = t.Degination,
                            ProfilePicture = t.ProfilePicture,
                            PartnerId = t.PartnerId,
                            CreatedDate = t.CreatedDate,
                            ModifiedDate = t.ModifiedDate,
                            PartnerName =  _unitOfWork.IUsersRepo.Table.Where(a => a.UserId == t.PartnerId).FirstOrDefault().UserName,
                            assignJobCount = _unitOfWork.IDocumentRepo.Table.Where(a => a.AssignedTeamId == t.Id).Count()

                        };
            if (query != null)
            {
                return query.ToList();
            }
            return null;
        }
        
        public List<TeamVM> GetTeamListByPartnerId(Guid id)
        {
            var query = from t in _unitOfWork.ITeamRepo.Table.Where(a=> a.PartnerId == id).OrderByDescending(a => a.CreatedDate)
                        select new TeamVM
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Degination = t.Degination,
                            ProfilePicture = t.ProfilePicture,
                            PartnerId = t.PartnerId,
                            CreatedDate = t.CreatedDate,
                            ModifiedDate = t.ModifiedDate,
                            PartnerName = _unitOfWork.IUsersRepo.Table.Where(a => a.UserId == t.PartnerId).FirstOrDefault().UserName,
                            assignJobCount = _unitOfWork.IDocumentRepo.Table.Where(a => a.AssignedTeamId == t.Id).Count()
                        };
            if (query != null)
            {
                return query.ToList();
            }
            return null;
        }

        public TeamVM GetById(int id)
        {
            var model = _unitOfWork.ITeamRepo.Table.Where(a => a.Id == id).FirstOrDefault();
            if (model != null)
            {
                var vm = new TeamVM()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Degination = model.Degination,
                    ProfilePicture = model.ProfilePicture,
                    PartnerId = model.PartnerId,
                    CreatedDate = model.CreatedDate,
                    PartnerName = _unitOfWork.IUsersRepo.Table.Where(a => a.UserId == model.PartnerId).FirstOrDefault().UserName,
                    assignJobCount = _unitOfWork.IDocumentRepo.Table.Where(a => a.AssignedTeamId == model.Id).Count()
                };
                return vm;
            }
            return null;
        }

        public void Insert(TeamVM model)
        {
            if (model != null)
            {
                var m = new Team()
                {
                    Name = model.Name,
                    Degination =model.Degination,
                    ProfilePicture = model.ProfilePicture,
                    PartnerId = model.PartnerId,
                    CreatedDate = DateTime.Now
                };
                _unitOfWork.ITeamRepo.Add(m);
                _unitOfWork.ITeamRepo.Save();
            }
        }

        public void Update(TeamVM model)
        {
            if (model != null)
            {
                var m = _unitOfWork.ITeamRepo.Table.Where(a => a.Id == model.Id).FirstOrDefault();
                m.Name = model.Name;
                m.Degination = model.Degination;
                m.ProfilePicture = model.ProfilePicture;
                m.PartnerId = model.PartnerId;
                m.ModifiedDate = DateTime.Now;
                _unitOfWork.ITeamRepo.Update(m);
                _unitOfWork.ITeamRepo.Save();
            }
        }
    }
}
