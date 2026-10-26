using BTTH2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using THanh1.Models;

namespace THanh1.Controllers
{
    [Route("Admin/Student")]
    public class StudentController : Controller
    {
        // Dùng static để lưu dữ liệu tạm thời khi thêm mới mà không bị reset
        private static List<Student> listStudents = new List<Student>();
        private readonly IWebHostEnvironment _env;

        // Inject IWebHostEnvironment qua Constructor
        public StudentController(IWebHostEnvironment env)
        {
            _env = env;

            // Khởi tạo 4 sinh viên mẫu nếu danh sách đang rỗng
            if (!listStudents.Any())
            {
                listStudents = new List<Student>()
                {
                    new Student() { Id = 101, Name = "Hải Nam", Branch = Branch.IT, Gender = Gender.Male, IsRegular = true, Address = "A1-2018", Email = "nam@g.com" },
                    new Student() { Id = 102, Name = "Minh Tú", Branch = Branch.BE, Gender = Gender.Female, IsRegular = true, Address = "A1-2019", Email = "tu@g.com" },
                    new Student() { Id = 103, Name = "Hoàng Phong", Branch = Branch.CE, Gender = Gender.Male, IsRegular = false, Address = "A1-2019", Email = "phong@g.com" },
                    new Student() { Id = 104, Name = "Xuân Mai", Branch = Branch.EE, Gender = Gender.Female, IsRegular = false, Address = "A1-2021", Email = "mai@g.com" }
                };
            }
        }

        [HttpGet("List", Name = "student_list")]
        [Route("")]
        public IActionResult Index()
        {
            return View(listStudents);
        }

        // Thêm phương thức GET để mở giao diện Create (tránh lỗi khi bấm vào thêm mới)
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem { Text = "IT", Value = "IT" },
                new SelectListItem { Text = "BE", Value = "BE" },
                new SelectListItem { Text = "CE", Value = "CE" },
                new SelectListItem { Text = "EE", Value = "EE" }
            };
            return View();
        }

        // Giữ lại hàm POST xử lý upload file khớp với action="/Admin/Student/Add" trong form của bạn
        [HttpPost("Add")]
        public async Task<IActionResult> Create(Student s)
        {
            if (ModelState.IsValid)
            {
                // Xử lý upload ảnh nếu người dùng có chọn file
                if (s.AvatarFile != null && s.AvatarFile.Length > 0)
                {
                    string uploadDir = Path.Combine(_env.WebRootPath, "images");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(s.AvatarFile.FileName);
                    string filePath = Path.Combine(uploadDir, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await s.AvatarFile.CopyToAsync(fileStream);
                    }

                    s.AvatarUrl = "/images/" + fileName;
                }

                s.Id = listStudents.Any() ? listStudents.Last().Id + 1 : 101;
                listStudents.Add(s);

                return View("Index", listStudents);
            }

            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem { Text = "IT", Value = "IT" },
                new SelectListItem { Text = "BE", Value = "BE" },
                new SelectListItem { Text = "CE", Value = "CE" },
                new SelectListItem { Text = "EE", Value = "EE" }
            };

            return View(s);
        }

    }

}