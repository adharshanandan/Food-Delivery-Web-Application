using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace FoodDeliveryWebApplication.Models
{
    public class User:BaseEntity
    {
        [Required]
        public string Name { get; set; }  
        public string Image { get; set; }
        [Required]
        [DisplayName("Profile picture")]
        [ValidateFileAttribute]
        public HttpPostedFileBase ImgUrl { get; set; }
        public string Status { get; set; }
        [Required]
        [RegularExpression("^[6][7-9]{1}[0-9]{4}$",ErrorMessage ="Invalid Picode")]
      
        public string Pincode { get; set; }


        public class ValidateFileAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                int maxContentLength = 1024 * 1024 * 2; //Max 2 MB is allowed
                string[] allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png" };
                var file = value as HttpPostedFileBase;
                if (file == null)
                {
                    return false;
                }
                else if (!allowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Image extension should be .jpg, .jpeg or .png";
                    return false;
                }
                else if (file.ContentLength > maxContentLength)
                {
                    ErrorMessage = "Your photo size is too large. Please upload image of size below 2 MB";
                    return false;
                }
                else
                {
                    return true;
                }



            }
        }


    }
}