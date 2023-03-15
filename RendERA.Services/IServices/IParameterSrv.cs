using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.ServiceManager.IServices
{
    public interface IParameterSrv
    {
        #region documnet parameter mapping

        DB.ViewModels.DocParametersMapVM SetParameterByDocId(DB.ViewModels.DocParametersMapVM model);
        DB.ViewModels.DocParametersMapVM GetSelectedParameterByDocId(int docId);
        DB.ViewModels.AdditionalParameterVM GetAdditionalParaByDocId(int documnetId, int? userCategoryId);
        void InsertDropdownDocParaMapping(int documentId, DB.ViewModels.DropDown dropDown);
        void InsertInputDocParaMapping(int documentId, DB.ViewModels.Input input);

        List<DB.ViewModels.ParametersVM> GetCPUList();
        List<DB.ViewModels.ParametersVM> Get2XCPUList();
        List<DB.ViewModels.ParametersVM> GetProcessorModelList();
        List<DB.ViewModels.ParametersVM> GetProcessorTypeList();
        DB.ViewModels.ParametersVM GetRenderTimeOfFrame();
        DB.ViewModels.ParametersVM GetNodesForProjectRederding();


        #endregion

        #region parameter
        void Insert(DB.ViewModels.ParametersVM model);

        int InsertParameter(DB.ViewModels.ParametersVM model);
        void Update(DB.ViewModels.ParametersVM model);
        void Delete(int id);
        List<DB.ViewModels.ParametersVM> GetAll();
      
        DB.ViewModels.ParametersVM GetById(int id);
        #endregion

        #region parameter software mapping
        List<DB.ViewModels.ParameterRenderSoftwaresMapVM> GetSelectedParameterOfSoftMapByParameterId(int parameterId);
        void AddRemoveSelectedParameterOfSoftMap(DB.ViewModels.ParametersVM model);
        List<DB.ViewModels.ParameterRenderSoftwaresMapVM> GetAllSoftwareParameterlist();
        #endregion

        #region Coupon Code
        DB.ViewModels.ParametersVM validateCouponCode(string coupon);
        #endregion

        #region User Category
        List<DB.ViewModels.UserCategoryMapVM> GetAllUserCategorylist();

        List<DB.ViewModels.UserCategoryMapVM> GetSelectedParameterOfUserCategoryByParameterId(int parameterId);
        void AddRemoveSelectedParameterOfUserCategory(DB.ViewModels.ParametersVM model);
        #endregion
    }
}
