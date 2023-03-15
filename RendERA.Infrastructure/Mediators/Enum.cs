using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RendERA.Infrastructure
{
    public static class Enum
    {
        public static string GetEnumDescription(this System.Enum value)
        {//this function will return enum description
            System.Reflection.FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi == null) {
                return " ";
            }
            try
            {
                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return value.ToString();
            }
            catch{
                return " ";
            }
        }

        public enum UserType
        {
            [Description("Admin")]
            Admin = 1,
            [Description("Partner")]
            Partner = 2,
            [Description("User")]
            User = 3

        }

        public enum UserCategory {
            [Description("Expert")]
            Expert = 1,
            [Description("Pro")]
            Pro = 2,
            [Description("Beginner ")]
            Beginner = 3
        }
        public enum RenderaSoftware
        {
            Maya = 1,
            ThreeDMax = 2,
            Arnold = 3,
            Cinema4D = 4,
            Blender = 5,
            VR = 6,
            Clarisse = 7,
            Nuke = 8,
            Others = 0
        }
        public enum SessionKey
        {
            RegisterationVerificationCode,
            LoggedInUserId
        }
        public enum Status
        {
            [Description("In Progress")]
            Inprogress = 1,
            [Description("Completed")]
            Completed = 2
        }
        public enum JobTrackingStatus {
            [Description("New Job")]
            NewJob = 1,
            [Description("In Progress")]
            InProgress = 2,
            [Description("Submitted")]
            Submitted = 3,
            [Description("Under Process")]
            UnderProcess = 4,
            [Description("Completed")]
            Completed = 5
        }
        public enum Category
        {
            [Description("1XCPU")]
            CPU = 1,
            [Description("2XCPU")]
            CPU2X = 2,
            [Description("Nodes For Project Renderding")]
            NodesForProjectRenderding = 3,
            [Description("Render Time Of Frame")]
            RenderTimeOfFrame = 4,
            [Description("Additional Parameter")]
            AdditionalParameter = 5,
            [Description("coupon code")]
            CouponCode = 6,
            [Description("Processor Model")]
            ProcessorModel = 7,
            [Description("Processor Type")]
            ProcessorType = 8
        }

        public enum CpuType
        {
            [Description("CPU")]
            CPU = 1,
            [Description("2XCPU")]
            CPU2X = 2
        }


        public enum EmailType
        {
            //to get value of description call  this function GetEnumDescription  

            [Description("Missed Payment")]
            MissedPayment = 1,

            [Description("Reminder - Automatic Payment")]
            ReminderAutomaticPayment = 2,

            [Description("Reminder - Manual Payment")]
            ReminderManualPayment = 3,

            [Description("Payment Confirmation Email")]
            PaymentConfirmationEmail = 4,

            [Description("Confirming your service cancellation Email")]
            ServiceCancellation = 5,

            [Description("Receipt Email")]
            InvoiceEmail = 6

        }
    }


    
}
