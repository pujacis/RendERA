using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace RendERA.DB.ViewModels
{
    public partial class ParametersVM
    {
        public ParametersVM()
        {
            CategoryList = new List<SelectListItem>();
        }
        public bool selected { get; set; }
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public int? Category { get; set; }
        public string CatergoryName { get; set;}
        public int? UserCategory { get; set; }
        public string Value { get; set; }
        public IList<SelectListItem> CategoryList { get; set; }
        public List<UserCategoryMapVM> UserCategoryList { get; set; }
        public List<ParameterRenderSoftwaresMapVM> RenderSoftwareList { get; set; }
    }

    public partial class DocParametersMapVM
    {
        public PaymentTransactionVM Transaction { get; set; } 
        public bool Selected { get; set; }
        public bool IsParameter { get; set; }
        public bool IsSubmitted { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }

        public int DocumentId { get; set; }
        public List<ParametersVM> List { get; set; }
        public List<ParametersVM> CPUList { get; set; }
        public List<ParametersVM> ProcessorModelList { get; set; }
        public List<ParametersVM> ProcessorTypeList { get; set; }
        public List<ParametersVM> TwoXCPUList { get; set; }
        public int SelectedCPU { get; set; }
        public int SelectedProcessorModel { get; set; }
        public int SelectedProcessorType { get; set; }
        public bool Is2XCPU { get; set; }
        public decimal SelectedPriceOfCPU { get; set; }
        public int NumberOfFrames { get; set; }
        public int TotalRenderTimeInSec { get; set; }
        public decimal PriceOfRenderTimePerSec { get; set; }
        public int NumberOfNodeForProjectRender { get; set; }
        public decimal PriceOfPerNodeForProjectRender { get; set; }
    
    
    }

    public partial class ParameterRenderSoftwaresMapVM {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }

    public partial class UserCategoryMapVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }

    public class AdditionalParameterVM{
        public int DocumentId { get; set; }
        public bool IsSubmitted { get; set; }
        public List<Input> InputFields { get; set; }
        public List<DropDown> DropDownFields { get; set; }
    }

    public class DropDown
    {
        public string LabelName { get; set; }
        public List<OptionList> DropDownOptionList { get; set; }
        public int Seletedvalue { get; set; }

    }

    public class OptionList
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class Input
    {
        public int Id { get; set; }
        public string LabelName { get; set; }
        public string Value { get; set; }
    }

}
