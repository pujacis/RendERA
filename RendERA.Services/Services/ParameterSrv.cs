using RendERA.DB.Models;
using RendERA.DB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RendERA.ServiceManager.Services
{

    public class ParameterSrv : IServices.IParameterSrv
    {
        private readonly RendERA.Infrastructure.IRepositories.IUnitOfWork _unitOfWork;
        public ParameterSrv(Infrastructure.IRepositories.IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        #region Parameter Document Mapping
        public void InsertDropdownDocParaMapping(int documentId, DropDown dropDown) {
            ParameterDocumnetMapping dataModel = new ParameterDocumnetMapping() { 
                DocumentId = documentId,
                ParameterId = dropDown.Seletedvalue,
                Frequency = 0,
                Price = "0"
            };
            var parameterDocumnetMapping = _unitOfWork.IParameterDocumnetMappingRepo.Table.Where(a=> a.DocumentId == documentId && a.Parameter.Name == dropDown.LabelName).FirstOrDefault();
            if (parameterDocumnetMapping != null) {
                DeleteDocParaMapping(parameterDocumnetMapping);
            }
            InsertDocParaMapping(dataModel);
        }

        public void InsertInputDocParaMapping(int documentId, Input input) {
            ParameterDocumnetMapping dataModel = new ParameterDocumnetMapping(){
                DocumentId = documentId,
                ParameterId = input.Id,
                Frequency = 0,
                Price = input.Value
            };
            var parameterDocumnetMapping = _unitOfWork.IParameterDocumnetMappingRepo.Table.Where(a => a.DocumentId == documentId && a.Parameter.Id == input.Id).FirstOrDefault();
            if (parameterDocumnetMapping != null)
            {
                DeleteDocParaMapping(parameterDocumnetMapping);
            }
            InsertDocParaMapping(dataModel);
        }

        public void DeleteDocParaMapping(ParameterDocumnetMapping model)
        {
            var objlist = _unitOfWork.IParameterDocumnetMappingRepo.Table.Where(a => a.ParameterId == model.ParameterId && a.DocumentId == model.DocumentId).FirstOrDefault();
            _unitOfWork.IParameterDocumnetMappingRepo.RemoveObj(objlist);
            _unitOfWork.IParameterDocumnetMappingRepo.Save();
        }

        public void InsertDocParaMapping(ParameterDocumnetMapping model)
        {
            if (model != null)
            {
                var parameterMapModel = new ParameterDocumnetMapping
                {
                   DocumentId = model.DocumentId,
                   ParameterId = model.ParameterId,
                   Frequency = model.Frequency,
                   Price = model.Price
                };
                _unitOfWork.IParameterDocumnetMappingRepo.Add(parameterMapModel);
                _unitOfWork.IParameterDocumnetMappingRepo.Save();
            }
        }

        public void UpdateDocParaMapping(ParameterDocumnetMapping model)
        {
            if (model != null)
            {
                var datamodel =_unitOfWork.IParameterDocumnetMappingRepo.Table.Where(a => a.DocumentId == model.DocumentId && a.ParameterId == model.ParameterId).FirstOrDefault();

                datamodel.Frequency = model.Frequency;
                datamodel.Price = model.Price;
                _unitOfWork.IParameterDocumnetMappingRepo.Update(datamodel);
                _unitOfWork.IParameterDocumnetMappingRepo.Save();
            }
        }

        public DocParametersMapVM GetSelectedParameterByDocId(int docId)
        {
            DocParametersMapVM docParametermodel = new DocParametersMapVM();
            var parameterlist = GetAll(); //get all parameter list;
            if (parameterlist != null && docId > 0)
            {
                var list = _unitOfWork.IParameterDocumnetMappingRepo.Table.Where(a => a.DocumentId == docId).ToList();
                foreach (var l in list)
                {
                    var obj = parameterlist.Find(a => a.Id == l.ParameterId && a.Category == (int)RendERA.Infrastructure.Enum.Category.CPU);
                    if (obj != null)
                    {
                        docParametermodel.SelectedPriceOfCPU = obj.Price;
                        docParametermodel.NumberOfFrames = l.Frequency;
                        docParametermodel.SelectedCPU = l.ParameterId;
                        docParametermodel.Is2XCPU = false;
                    }
                    obj = parameterlist.Find(a => a.Id == l.ParameterId && a.Category == (int)RendERA.Infrastructure.Enum.Category.CPU2X);
                    if (obj != null)
                    {
                        docParametermodel.SelectedPriceOfCPU = System.Convert.ToDecimal(l.Price);
                        docParametermodel.NumberOfFrames = l.Frequency;
                        docParametermodel.SelectedCPU = l.ParameterId;
                        docParametermodel.Is2XCPU = true;
                    }
                    obj = parameterlist.Find(a => a.Id == l.ParameterId && a.Category == (int)RendERA.Infrastructure.Enum.Category.ProcessorModel);
                    if (obj != null)
                    {
                        //docParametermodel.SelectedProcessorModel = System.Convert.ToDecimal(l.Price);
                        docParametermodel.NumberOfFrames = l.Frequency;
                        docParametermodel.SelectedProcessorModel = l.ParameterId;
                        //docParametermodel.Is2XCPU = true;
                    }
                    obj = parameterlist.Find(a => a.Id == l.ParameterId && a.Category == (int)RendERA.Infrastructure.Enum.Category.ProcessorType);
                    if (obj != null)
                    {
                       // docParametermodel.SelectedPriceOfCPU = System.Convert.ToDecimal(l.Price);
                        docParametermodel.NumberOfFrames = l.Frequency;
                        docParametermodel.SelectedProcessorType = l.ParameterId;
                       // docParametermodel.Is2XCPU = true;
                    }
                    obj = parameterlist.Find(a => a.Id == l.ParameterId && a.Category == (int)RendERA.Infrastructure.Enum.Category.NodesForProjectRenderding);
                    if (obj != null)
                    {
                        docParametermodel.NumberOfNodeForProjectRender = l.Frequency;
                        docParametermodel.PriceOfPerNodeForProjectRender = System.Convert.ToDecimal(l.Price);
                    }
                    obj = parameterlist.Find(a => a.Id == l.ParameterId && a.Category == (int)RendERA.Infrastructure.Enum.Category.RenderTimeOfFrame);
                    if (obj != null)
                    {
                        docParametermodel.TotalRenderTimeInSec = l.Frequency;
                        docParametermodel.PriceOfRenderTimePerSec = System.Convert.ToDecimal(l.Price);
                    }
                    obj = parameterlist.Find(a => a.Id == l.ParameterId && a.Category == (int)RendERA.Infrastructure.Enum.Category.NodesForProjectRenderding);
                    if (obj != null)
                    {
                        docParametermodel.NumberOfNodeForProjectRender = l.Frequency;
                        docParametermodel.PriceOfPerNodeForProjectRender = System.Convert.ToDecimal(l.Price);
                    }
                    obj = parameterlist.Find(a => a.Id == l.ParameterId && a.Category == (int)RendERA.Infrastructure.Enum.Category.RenderTimeOfFrame);
                    if (obj != null)
                    {
                        docParametermodel.TotalRenderTimeInSec = l.Frequency;
                        docParametermodel.PriceOfRenderTimePerSec = System.Convert.ToDecimal(l.Price);
                    }
                }
            }
            return docParametermodel;
        }

        public DocParametersMapVM SetParameterByDocId(DocParametersMapVM model)
        {
            DocParametersMapVM docParametermodel = new DocParametersMapVM();
            var parameterlist = GetAll(); //get all parameter list;
            if (parameterlist != null && model.DocumentId > 0)
            {
                var list = _unitOfWork.IParameterDocumnetMappingRepo.Table.Where(a => a.DocumentId == model.DocumentId).ToList();
                if (model.Is2XCPU == false)
                {
                    var cpuParameter = _unitOfWork.IParameterDocumnetMappingRepo.Table.Where(a => a.DocumentId == model.DocumentId && a.Parameter.Category == (int)RendERA.Infrastructure.Enum.Category.CPU).FirstOrDefault();
                    if (cpuParameter != null)
                    {
                        DeleteDocParaMapping(cpuParameter);
                        var setparameter = new ParameterDocumnetMapping();
                        setparameter.Frequency = model.NumberOfFrames;
                        setparameter.Price = model.SelectedPriceOfCPU.ToString();
                        setparameter.DocumentId = model.DocumentId;
                        setparameter.ParameterId = model.SelectedCPU;
                        InsertDocParaMapping(setparameter);

                    }
                    else
                    {
                        var setparameter = new ParameterDocumnetMapping();
                        setparameter.Frequency = model.NumberOfFrames;
                        setparameter.Price = model.SelectedPriceOfCPU.ToString();
                        setparameter.DocumentId = model.DocumentId;
                        setparameter.ParameterId = model.SelectedCPU;
                        InsertDocParaMapping(setparameter);
                    }
                }
                else {
                    var cpu2xParameter = _unitOfWork.IParameterDocumnetMappingRepo.Table.Where(a => a.DocumentId == model.DocumentId && a.Parameter.Category == (int)RendERA.Infrastructure.Enum.Category.CPU2X).FirstOrDefault();
                    if (cpu2xParameter != null)
                    {
                        DeleteDocParaMapping(cpu2xParameter);
                        var setparameter = new ParameterDocumnetMapping();
                        setparameter.Frequency = model.NumberOfFrames;
                        setparameter.Price = model.SelectedPriceOfCPU.ToString();
                        setparameter.DocumentId = model.DocumentId;
                        setparameter.ParameterId = model.SelectedCPU;
                        InsertDocParaMapping(setparameter);
                    }
                    else
                    {
                        var setparameter = new ParameterDocumnetMapping();
                        setparameter.Frequency = model.NumberOfFrames;
                        setparameter.Price = model.SelectedPriceOfCPU.ToString();
                        setparameter.DocumentId = model.DocumentId;
                        setparameter.ParameterId = model.SelectedCPU;
                        InsertDocParaMapping(setparameter);
                    }
                }
                var RenderNode = _unitOfWork.IParameterDocumnetMappingRepo.Table.Where(a=> a.DocumentId == model.DocumentId && a.Parameter.Category == (int)RendERA.Infrastructure.Enum.Category.NodesForProjectRenderding).FirstOrDefault();
                if (RenderNode != null)
                {

                    RenderNode.Frequency = model.NumberOfNodeForProjectRender;
                    RenderNode.Price = model.PriceOfPerNodeForProjectRender.ToString();
                    RenderNode.DocumentId = model.DocumentId;
                    UpdateDocParaMapping(RenderNode);
                }
                else {
                    var RenderNodeparameter = _unitOfWork.IParameterRepo.Table.Where(a => a.Category == (int)RendERA.Infrastructure.Enum.Category.NodesForProjectRenderding).FirstOrDefault();
                    RenderNode = new ParameterDocumnetMapping();
                    RenderNode.Frequency = model.NumberOfNodeForProjectRender;
                    RenderNode.Price = model.PriceOfPerNodeForProjectRender.ToString();
                    RenderNode.DocumentId = model.DocumentId;
                    RenderNode.ParameterId = RenderNodeparameter.Id;
                    InsertDocParaMapping(RenderNode);
                }
                var FrameTime = _unitOfWork.IParameterDocumnetMappingRepo.Table.Where(a => a.DocumentId == model.DocumentId && a.Parameter.Category == (int)RendERA.Infrastructure.Enum.Category.RenderTimeOfFrame).FirstOrDefault();
                if (FrameTime != null)
                {

                    FrameTime.Frequency = model.TotalRenderTimeInSec;
                    FrameTime.Price = model.PriceOfRenderTimePerSec.ToString();
                    FrameTime.DocumentId = model.DocumentId;
                    UpdateDocParaMapping(FrameTime);
                }
                else
                {
                    var RenderNodeparameter = _unitOfWork.IParameterRepo.Table.Where(a => a.Category == (int)RendERA.Infrastructure.Enum.Category.RenderTimeOfFrame).FirstOrDefault();
                    FrameTime = new ParameterDocumnetMapping();
                    FrameTime.Frequency = model.TotalRenderTimeInSec;
                    FrameTime.Price = model.PriceOfRenderTimePerSec.ToString();
                    FrameTime.DocumentId = model.DocumentId;
                    FrameTime.ParameterId = RenderNodeparameter.Id;
                    InsertDocParaMapping(FrameTime);
                }
                var processorModel = _unitOfWork.IParameterDocumnetMappingRepo.Table.Where(a => a.DocumentId == model.DocumentId && a.Parameter.Category == (int)RendERA.Infrastructure.Enum.Category.ProcessorModel).FirstOrDefault();
                if (processorModel != null)
                {
                    DeleteDocParaMapping(processorModel);
                    var setparameter = new ParameterDocumnetMapping();
                    setparameter.Frequency = model.NumberOfFrames;
                    setparameter.Price = model.SelectedProcessorModel.ToString();
                    setparameter.DocumentId = model.DocumentId;
                    setparameter.ParameterId = model.SelectedProcessorModel;
                    InsertDocParaMapping(setparameter);

                }
                else
                {
                    var setparameter = new ParameterDocumnetMapping();
                    setparameter.Frequency = model.NumberOfFrames;
                    setparameter.Price = model.SelectedProcessorModel.ToString();
                    setparameter.DocumentId = model.DocumentId;
                    setparameter.ParameterId = model.SelectedProcessorModel;
                    InsertDocParaMapping(setparameter);
                }
                var ProcessorType = _unitOfWork.IParameterDocumnetMappingRepo.Table.Where(a => a.DocumentId == model.DocumentId && a.Parameter.Category == (int)RendERA.Infrastructure.Enum.Category.ProcessorType).FirstOrDefault();
                if (ProcessorType != null)
                {
                    DeleteDocParaMapping(ProcessorType);
                    var setparameter = new ParameterDocumnetMapping();
                    setparameter.Frequency = model.NumberOfFrames;
                    setparameter.Price = model.SelectedProcessorType.ToString();
                    setparameter.DocumentId = model.DocumentId;
                    setparameter.ParameterId = model.SelectedProcessorType;
                    InsertDocParaMapping(setparameter);

                }
                else
                {
                    var setparameter = new ParameterDocumnetMapping();
                    setparameter.Frequency = model.NumberOfFrames;
                    setparameter.Price = model.SelectedProcessorType.ToString();
                    setparameter.DocumentId = model.DocumentId;
                    setparameter.ParameterId = model.SelectedProcessorType;
                    InsertDocParaMapping(setparameter);
                }
            }
            return docParametermodel;
        }


        public AdditionalParameterVM GetAdditionalParaByDocId(int documnetId, int? userCategoryId) {
            var model = new AdditionalParameterVM();
            var dropDownkeys =  _unitOfWork.IParameterRepo.Table.GroupBy(c => c.Name).Where(grp => grp.Count() > 1).ToList();
            
            //preapare dropdownlist
            if (dropDownkeys != null) {
                List<DropDown> dropDownFields = new List<DropDown>();
                foreach (var Igrouping in dropDownkeys) {
                 //  var additionalPara = _unitOfWork.IParameterRepo.Table.Where(a=> a.Name == Igrouping.Key && a.Category == (int)RendERA.Infrastructure.Enum.Category.AdditionalParameter).ToList();

                    var additionalPara = (from pd in _unitOfWork.IParameterRepo.Table
                             join od in _unitOfWork.IParameterUserCategoryMappingRepo.Table on pd.Id equals od.ParameterId where pd.Name == Igrouping.Key &&  od.UserCategoryId == userCategoryId && pd.Category == (int)RendERA.Infrastructure.Enum.Category.AdditionalParameter
                             orderby pd.Id
                             select new Parameter
                             {
                                 Id = pd.Id,
                                 Category = pd.Category,
                                 Name = pd.Name,
                                 Price =pd.Price,
                                 Value =pd.Value,
                                 CreatedDate =pd.CreatedDate,
                                 ModifiedDate =pd.ModifiedDate
                             }).ToList();
                    //var additionalPara1 = _unitOfWork.IParameterRepo.Table.Where(a=> a.Name == Igrouping.Key && a.Category == (int)RendERA.Infrastructure.Enum.Category.AdditionalParameter).ToList();
                    if (additionalPara.Count  > 0)
                    {
                        var dropDown = new DropDown();
                        dropDown.LabelName = Igrouping.Key;
                        //fill option list of dropdown
                        List<OptionList> dropDownOptionList = new List<OptionList>();
                        foreach (var p in additionalPara)
                        {
                            var Seletedvalue = _unitOfWork.IParameterDocumnetMappingRepo.Table.Where(a => a.ParameterId == p.Id && a.DocumentId == documnetId).FirstOrDefault();
                            if (Seletedvalue != null)
                            {
                                dropDown.Seletedvalue = Seletedvalue.ParameterId;
                            }
                            dropDownOptionList.Add(new OptionList { Id = p.Id, Name = p.Value });
                        }
                        dropDown.DropDownOptionList = dropDownOptionList;
                        dropDownFields.Add(dropDown);
                    }
                }
               model.DropDownFields = dropDownFields;
            }

            //preapare inputlist
            
            var  inputFieldKeys = _unitOfWork.IParameterRepo.Table.GroupBy(c => c.Name).Where(grp => grp.Count() == 1).ToList();
            if (inputFieldKeys != null) {
                List<Input> inputFields = new List<Input>();
                foreach (var Igrouping in inputFieldKeys)
                {
                   // var additionalPara = _unitOfWork.IParameterRepo.Table.Where(a => a.Name == Igrouping.Key && a.Category == (int)RendERA.Infrastructure.Enum.Category.AdditionalParameter).FirstOrDefault();
                    var additionalPara = (from pd in _unitOfWork.IParameterRepo.Table
                                          join od in _unitOfWork.IParameterUserCategoryMappingRepo.Table on pd.Id equals od.ParameterId
                                          where pd.Name == Igrouping.Key && od.UserCategoryId == userCategoryId && pd.Category == (int)RendERA.Infrastructure.Enum.Category.AdditionalParameter
                                          orderby pd.Id
                                          select new Parameter
                                          {
                                              Id = pd.Id,
                                              Category = pd.Category,
                                              Name = pd.Name,
                                              Price = pd.Price,
                                              Value = pd.Value,
                                              CreatedDate = pd.CreatedDate,
                                              ModifiedDate = pd.ModifiedDate
                                          }).FirstOrDefault();
                    if (additionalPara != null) {
                        var input = new Input();
                        input.LabelName = Igrouping.Key;
                        input.Id = additionalPara.Id;
                        var value = _unitOfWork.IParameterDocumnetMappingRepo.Table.Where(a => a.ParameterId == additionalPara.Id && a.DocumentId == documnetId).FirstOrDefault();
                        if (value != null) {
                            input.Value = value.Price;
                        }
                        inputFields.Add(input);
                    }
                }
                model.InputFields = inputFields;
            }
            return model;
        }

        #endregion

        #region parameter
        public void Delete(int  id)
        {
            if (GetById(id) != null)
            {
                _unitOfWork.IParameterRepo.Remove(id);
                _unitOfWork.IParameterRepo.Save();
            }
        }



        public List<ParametersVM> GetAll()
        {
            var query = from d in _unitOfWork.IParameterRepo.Table.OrderByDescending(a => a.Name)
                        select new ParametersVM
                        {
                            Id = d.Id,
                            Name = d.Name,
                            Price = d.Price,
                            Category = d.Category,
                            Value = d.Value,
                            CatergoryName = _unitOfWork.IMasterCategoryRepo.Table.Where(a => a.Id == d.Category).FirstOrDefault() != null ?
                            _unitOfWork.IMasterCategoryRepo.Table.Where(a => a.Id == d.Category).FirstOrDefault().Name
                            :Enum.GetName(typeof(RendERA.Infrastructure.Enum.Category), d.Category)
                        };
            if (query != null) 
            {
                return query.ToList();
            }
            return null;
        }

        public ParametersVM GetById(int  id)
        {
            var p = _unitOfWork.IParameterRepo.Table.Where(a=> a.Id == id).FirstOrDefault();
            if (p != null)
            {
                var vm = new ParametersVM()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category,
                    Value =p.Value
                };
                return vm;
            }
            return null;
        }

        public void Insert(ParametersVM model)
        {
            if (model != null)
            {
                var m = new Parameter()
                {
                    Name = model.Name,
                    Price = model.Price,
                    Category = model.Category,
                    CreatedDate = DateTime.Now,
                    Value = model.Value
                };
                _unitOfWork.IParameterRepo.Add(m);
                _unitOfWork.IParameterRepo.Save();
            }
        } 
        public int InsertParameter(ParametersVM model)
        {
            if (model != null)
            {
                var m = new Parameter()
                {
                    Name = model.Name,
                    Price = model.Price,
                    Category = model.Category,
                    CreatedDate = DateTime.Now,
                    Value = model.Value
                };
                _unitOfWork.IParameterRepo.Add(m);
                _unitOfWork.IParameterRepo.Save();
            return m.Id;
            }
            return 0;
        }

        public void Update(ParametersVM model)
        {
            if (model != null)
            {
                var m = _unitOfWork.IParameterRepo.Table.Where(a => a.Id == model.Id).FirstOrDefault();
                m.Name = model.Name;
                m.Price = model.Price;
                m.Category = model.Category;
                m.ModifiedDate = DateTime.Now;
                m.Value = model.Value;
                _unitOfWork.IParameterRepo.Update(m);
                _unitOfWork.IParameterRepo.Save();
            }
        }

        public List<ParametersVM> GetCPUList()
        {
            List<ParametersVM> CPUList = new List<ParametersVM>();
            var parameterlist = GetAll(); //get all parameter list;
            if (parameterlist != null )
            {
                foreach (var parameter in parameterlist)
                {
                    if (parameter.Category == (int)RendERA.Infrastructure.Enum.Category.CPU) {
                        CPUList.Add(parameter);
                    }
                }
            }
            return CPUList;
        }

        public List<ParametersVM> Get2XCPUList()
        {
            List<ParametersVM> TwoXCPUList = new List<ParametersVM>();
            var parameterlist = GetAll(); //get all parameter list;
            if (parameterlist != null )
            {
                foreach (var parameter in parameterlist)
                {
                    {
                        if (parameter.Category == (int)RendERA.Infrastructure.Enum.Category.CPU2X)
                        {
                            TwoXCPUList.Add(parameter);
                        }
                    }
                }
            }
            return TwoXCPUList;
        }
        public List<ParametersVM> GetProcessorModelList()
        {
            List<ParametersVM> CPUList = new List<ParametersVM>();
            var parameterlist = GetAll(); //get all parameter list;
            if (parameterlist != null )
            {
                foreach (var parameter in parameterlist)
                {
                    if (parameter.Category == (int)RendERA.Infrastructure.Enum.Category.ProcessorModel) {
                        CPUList.Add(parameter);
                    }
                }
            }
            return CPUList;
        }

        public List<ParametersVM> GetProcessorTypeList()
        {
            List<ParametersVM> TwoXCPUList = new List<ParametersVM>();
            var parameterlist = GetAll(); //get all parameter list;
            if (parameterlist != null )
            {
                foreach (var parameter in parameterlist)
                {
                    {
                        if (parameter.Category == (int)RendERA.Infrastructure.Enum.Category.ProcessorType)
                        {
                            TwoXCPUList.Add(parameter);
                        }
                    }
                }
            }
            return TwoXCPUList;
        }

        public ParametersVM GetRenderTimeOfFrame()
        {

           // List<ParametersVM> CPUList = new List<ParametersVM>();
            var parameterlist = GetAll(); //get all parameter list;
            if (parameterlist != null )
            {
               //   var list = _unitOfWork.IParameterDocumnetMappingRepo.Table.Where(a => a.DocumentId == docId).ToList();
                foreach (var parameter in parameterlist)
                {
                //    int index = list.FindIndex(item => item.DocumentId == docId && item.ParameterId == parameter.Id);
                 //   if (index >= 0)
                 //   {
                        if (parameter.Category == (int)RendERA.Infrastructure.Enum.Category.RenderTimeOfFrame)
                        {
                           // parameter.selectedPrice = list[index].SelectedPrice;
                            return parameter;
                        }
                  //  }
                }
            }
            return null;
        }

        public ParametersVM GetNodesForProjectRederding()
        {
            var parameterlist = GetAll(); //get all parameter list;
            if (parameterlist != null )
            {
                foreach (var parameter in parameterlist)
                {
                        if (parameter.Category == (int)RendERA.Infrastructure.Enum.Category.NodesForProjectRenderding)
                        {
                         
                            return parameter;
                        }
                }
            }
            return null;
        }

        #endregion

        #region parameter software mapping

        public void DeleteDocParaOfSoftMapping(ParameterSoftwareRenderMapping model)
        {
            var objlist = _unitOfWork.IParameterSoftwareMappingRepo.Table.Where(a => a.ParameterId == model.ParameterId && a.SoftwareRenderId == model.SoftwareRenderId).FirstOrDefault();
            _unitOfWork.IParameterSoftwareMappingRepo.RemoveObj(objlist);
            _unitOfWork.IParameterSoftwareMappingRepo.Save();
        }
        public void InsertDocParaOfSoftMapping(ParameterSoftwareRenderMapping model)
        {
            if (model != null)
            {
                var parameterMapModel = new ParameterSoftwareRenderMapping
                {
                    SoftwareRenderId = model.SoftwareRenderId,
                    ParameterId = model.ParameterId
                };
                _unitOfWork.IParameterSoftwareMappingRepo.Add(parameterMapModel);
                _unitOfWork.IParameterSoftwareMappingRepo.Save();
            }
        }
        public List<ParameterRenderSoftwaresMapVM> GetAllSoftwareParameterlist() {
            var softwareParameterlist = new List<ParameterRenderSoftwaresMapVM>();
            var elements = Enum.GetValues(typeof(RendERA.Infrastructure.Enum.RenderaSoftware)).Cast<RendERA.Infrastructure.Enum.RenderaSoftware>();
            foreach (var element in elements)
            {
                // adde all the Category in list  except other
                if (((int)element) != (int)RendERA.Infrastructure.Enum.RenderaSoftware.Others )
                {
                    softwareParameterlist.Add(new ParameterRenderSoftwaresMapVM
                    {
                        Id = (int)element,
                        Name = RendERA.Infrastructure.Enum.GetEnumDescription(element)
                    });
                }
            }
            return softwareParameterlist;
        }


        public List<ParameterRenderSoftwaresMapVM> GetSelectedParameterOfSoftMapByParameterId(int parameterId)
        {
          
            var softwareParameterlist = GetAllSoftwareParameterlist(); //get all parameter list;
            if (softwareParameterlist != null && parameterId > 0)
            {
                var list = _unitOfWork.IParameterSoftwareMappingRepo.Table.Where(a => a.ParameterId == parameterId).ToList();
                foreach (var softwareParameter in softwareParameterlist)
                {
                    if (list.FindIndex(item => item.ParameterId == parameterId && item.SoftwareRenderId == softwareParameter.Id) >= 0)
                    {
                        softwareParameter.Selected = true;
                    }
                }
            }
            return softwareParameterlist;
        }

        public void AddRemoveSelectedParameterOfSoftMap(ParametersVM model)
        {
           if (model != null)
            {
                var list = _unitOfWork.IParameterSoftwareMappingRepo.Table.Where(a => a.ParameterId == model.Id).ToList();
                foreach (var obj in model.RenderSoftwareList)
                {
                    var parameterDocumnetMap = list.Find(item => item.ParameterId == model.Id && item.SoftwareRenderId == obj.Id);
                    if (parameterDocumnetMap != null && obj.Selected == false)
                    {
                        var deleteobj = parameterDocumnetMap;
                        DeleteDocParaOfSoftMapping(deleteobj);
                    }
                    else if (parameterDocumnetMap == null && obj.Selected == true)
                    {
                        var insertobj = new ParameterSoftwareRenderMapping { SoftwareRenderId = obj.Id, ParameterId = model.Id };
                        InsertDocParaOfSoftMapping(insertobj);
                    }
                }
            }
        }



        #endregion

        #region Coupon Code
        public DB.ViewModels.ParametersVM validateCouponCode(string coupon)
        {
            if (coupon != null)
            {
                var couponParameter = _unitOfWork.IParameterRepo.Table.Where(a => a.Name == coupon && a.Category == (int)RendERA.Infrastructure.Enum.Category.CouponCode).FirstOrDefault();
                if (couponParameter != null) {
                    ParametersVM couponParameterVM = new ParametersVM() { 
                    Value = couponParameter.Value,
                    Id = couponParameter.Id,
                    Name = couponParameter.Name,
                    Price = couponParameter.Price,
                    Category = couponParameter.Category
                    };
                    return couponParameterVM;
                }
            }
            return null;
        }
        #endregion

        #region User Category

        public void InsertDocParaOfUserCategory(ParameterUserCategoryMapping model)
        {
            if (model != null)
            {
                var parameterMapModel = new ParameterUserCategoryMapping
                {
                    UserCategoryId = model.UserCategoryId,
                    ParameterId = model.ParameterId
                };
                _unitOfWork.IParameterUserCategoryMappingRepo.Add(parameterMapModel);
                _unitOfWork.IParameterUserCategoryMappingRepo.Save();
            }
        }

        public void DeleteDocParaOfUserCategory(ParameterUserCategoryMapping model)
        {
            var objlist = _unitOfWork.IParameterUserCategoryMappingRepo.Table.Where(a => a.ParameterId == model.ParameterId && a.UserCategoryId == model.UserCategoryId).FirstOrDefault();
            _unitOfWork.IParameterUserCategoryMappingRepo.RemoveObj(objlist);
            _unitOfWork.IParameterUserCategoryMappingRepo.Save();
        }
        public List<UserCategoryMapVM> GetAllUserCategorylist()
        {
            var userCategorylist = new List<UserCategoryMapVM>();
            var elements = Enum.GetValues(typeof(RendERA.Infrastructure.Enum.UserCategory)).Cast<RendERA.Infrastructure.Enum.UserCategory>();
            foreach (var element in elements)
            {
                userCategorylist.Add(new UserCategoryMapVM
                {
                    Id = (int)element,
                    Name = RendERA.Infrastructure.Enum.GetEnumDescription(element)
                });
            }
            return userCategorylist;
        }

        public List<UserCategoryMapVM> GetSelectedParameterOfUserCategoryByParameterId(int parameterId)
        {

            var UserCategorylist = GetAllUserCategorylist(); //get all parameter list;
            if (UserCategorylist != null && parameterId > 0)
            {
                var list = _unitOfWork.IParameterUserCategoryMappingRepo.Table.Where(a => a.ParameterId == parameterId).ToList();
                foreach (var userCategoryParameter in UserCategorylist)
                {
                    if (list.FindIndex(item => item.ParameterId == parameterId && item.UserCategoryId == userCategoryParameter.Id) >= 0)
                    {
                        userCategoryParameter.Selected = true;
                    }
                }
            }
            return UserCategorylist;
        }

        public void AddRemoveSelectedParameterOfUserCategory(ParametersVM model)
        {
            if (model != null)
            {
                var list = _unitOfWork.IParameterUserCategoryMappingRepo.Table.Where(a => a.ParameterId == model.Id).ToList();
                foreach (var obj in model.UserCategoryList)
                {
                    var parameterDocumnetMap = list.Find(item => item.ParameterId == model.Id && item.UserCategoryId == obj.Id);
                    if (parameterDocumnetMap != null && obj.Selected == false)
                    {
                        var deleteobj = parameterDocumnetMap;
                        DeleteDocParaOfUserCategory(deleteobj);
                    }
                    else if (parameterDocumnetMap == null && obj.Selected == true)
                    {
                        var insertobj = new ParameterUserCategoryMapping { UserCategoryId = obj.Id, ParameterId = model.Id };
                        InsertDocParaOfUserCategory(insertobj);
                    }
                }
            }
        }
        #endregion
    }

}