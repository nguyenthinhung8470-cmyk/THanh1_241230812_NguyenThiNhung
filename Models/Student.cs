using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using THanh1.Models;

namespace BTTH2.Models
{
    public class Student
    {
        public int Id { get; set; } //Mã sinh viên 
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "{0} phải từ {2} đến {1} ký tự")]
        public string? Name { get; set; }//Họ tên 

        [Required(ErrorMessage = "Enamil bắt buộc phải được nhập")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string? Email { get; set; }//Email

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "{0} phải có ít nhất {2} ký tự")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
             ErrorMessage = "{0} phải từ 8 ký tự trở lên, bao gồm chữ hoa, chữ thường, chữ số và ký tự đặc biệt")]
        public string? Password { get; set; } // Mật khẩu 

        [Display(Name = "Ngành học")]
        [Required(ErrorMessage = "Vui lòng chọn {0}")]
        public Branch? Branch { get; set; } // Ngành học 

        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Vui lòng chọn {0}")]
        public Gender? Gender { get; set; } // Giới tính 

        [Display(Name = "Hệ đào tạo")]
        public bool IsRegular { get; set; } // Hệ: true - chính quy, false - phi chính quy

        [Display(Name = "Địa chỉ")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string? Address { get; set; } // Địa chỉ 

        [Display(Name = "Ngày sinh")]
        [Range(typeof(DateTime), "1/1/1963", "12/31/2005", ErrorMessage = "{0} không hợp lệ")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "{0} không được để trống")]
        public DateTime DateOfBorth { get; set; } // Ngày sinh 

        [Display(Name = "Điểm")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [Range(0.0, 10.0, ErrorMessage = "{0} phải từ {1} đến {2}")]
        public float? Score { get; set; } // Điểm số (kiểu số thực từ 0.0 đến 10.0)

        // **Bổ sung thêm thuộc tính này để khớp với input type="file" trong Create.cshtml**
        [Display(Name = "Ảnh đại diện")]
        public IFormFile? AvatarFile { get; set; }
        public string? AvatarUrl { get; set; }
    }
}