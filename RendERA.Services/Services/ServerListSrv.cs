using RendERA.ServiceManager.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RendERA.ServiceManager.Services
{
    public class ServerListSrv : IServerListSrv
    {
        private readonly RendERA.Infrastructure.IRepositories.IUnitOfWork _unitOfWork;
        public ServerListSrv(Infrastructure.IRepositories.IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        public void Delete(int id)
        {
            if (GetById(id) != null)
            {
                _unitOfWork.IServerListRepo.Remove(id);
                _unitOfWork.IServerListRepo.Save();
            }
        }

        public List<DB.ViewModels.ServerListVM> GetAll()
        {
            var query = from t in _unitOfWork.IServerListRepo.Table.OrderByDescending(a => a.Name)
                        select new DB.ViewModels.ServerListVM
                        {
                            Id = t.Id,
                            Name = t.Name,
                            IpAddress = t.IpAddress
                        };
            if (query != null)
            {
                return query.ToList();
            }
            return null;
        }

        public DB.ViewModels.ServerListVM GetById(int id)
        {
            var model = _unitOfWork.IServerListRepo.Table.Where(a => a.Id == id).FirstOrDefault();
            if (model != null)
            {
                var vm = new DB.ViewModels.ServerListVM()
                {
                    Id = model.Id,
                    Name = model.Name,
                    IpAddress = model.IpAddress
                };
                return vm;
            }
            return null;
        }

        public void Insert(DB.ViewModels.ServerListVM model)
        {
            if (model != null)
            {
                var m = new DB.Models.ServerList()
                {
                    Name = model.Name,
                    IpAddress = model.IpAddress,
                    CreatedDate = DateTime.Now
                };
                _unitOfWork.IServerListRepo.Add(m);
                _unitOfWork.IServerListRepo.Save();
            }
        }

        public void Update(DB.ViewModels.ServerListVM model)
        {
            if (model != null)
            {
                var m = _unitOfWork.IServerListRepo.Table.Where(a => a.Id == model.Id).FirstOrDefault();
                m.Name = model.Name;
                m.IpAddress =model.IpAddress;
                m.ModifiedDate = DateTime.Now;
                _unitOfWork.IServerListRepo.Update(m);
                _unitOfWork.IServerListRepo.Save();
            }
        }
    }
}
