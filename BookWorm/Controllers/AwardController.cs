using BookWorm.Data.UnitOfWork;
using BookWorm.Models;
using BookWorm.Models.DTOs;
using BookWorm.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.Controllers
{
    public class AwardController : Controller
    {
        private const string NotFoundMessage = "The Page You Were Looking for Was Not Found!";
        private const string UnexpectedErrorMessage = "Something Unexpected Has Happened!";
        private readonly IUnitOfWork _unitOfWork;


        public AwardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        [HttpGet]
        public async Task<ViewResult> Details(int id)
        {
            try
            {
                var award = await _unitOfWork.AwardRepository.GetAwardWithPhotoAndBooksAsync(id);
                //TODO:log details
                return award != null ? View(award) : View("Error",new ErrorViewModel(NotFoundMessage));
            }
            catch (Exception e)
            {  
                //unexpected error
                return View("Error",new ErrorViewModel(UnexpectedErrorMessage));
            }
        }


        [HttpGet]
        public ViewResult Create() => View();



        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AwardDto awardDto)
        {
            try
            {
                var award = new Award()
                {
                    Name = awardDto.Name,
                    About = awardDto.About,
                    AwardPhoto = awardDto.PhotoFile == null ? null : new ApplicationImage()
                    {
                        FileName = awardDto.PhotoFile.FileName,
                        FileSize = awardDto.PhotoFile.Length
                    }
                };
                if (award.AwardPhoto != null)
                {
                    await FileManager.SavePhotoAsync(award.AwardPhoto.Id, awardDto.PhotoFile!);
                }

                await _unitOfWork.AwardRepository.AddAsync(award);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Details",new {id = award.Id});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
