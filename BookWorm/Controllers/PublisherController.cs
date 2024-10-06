using BookWorm.Data.UnitOfWork;
using BookWorm.Models.DTOs;
using BookWorm.Models;
using BookWorm.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.Controllers
{
    public class PublisherController : Controller
    {
        private const string NotFoundMessage = "The Page You Were Looking for Was Not Found!";
        private const string UnexpectedErrorMessage = "Something Unexpected Has Happened!";
        private readonly IUnitOfWork _unitOfWork;


        public PublisherController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<ViewResult> Details(int id)
        {
            try
            {
                var publisher = await _unitOfWork.PublisherRepository.GetPublisherWithPublishedBooksAsync(id);
                //TODO:log details
                return publisher != null ? View(publisher) : View("Error",new ErrorViewModel(NotFoundMessage));
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
        public async Task<IActionResult> Create([FromForm] PublisherDto publisherDto)
        {
            Console.WriteLine(publisherDto.About ?? "null from dto***********************");
            try
            {
                var publisher = new Publisher(publisherDto.Name)
                {
                    About = publisherDto.About,
                    PublisherLogo = publisherDto.PhotoFile == null ? null : new ApplicationImage()
                    {
                        FileName = publisherDto.PhotoFile.FileName,
                        FileSize = publisherDto.PhotoFile.Length
                    }
                };
                Console.WriteLine(publisher.About ?? "null in publisher**************************");
                if (publisher.PublisherLogo != null)
                {
                    await FileManager.SavePhotoAsync(publisher.PublisherLogo.Id, publisherDto.PhotoFile!);
                }
                await _unitOfWork.PublisherRepository.AddAsync(publisher);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Details", "Publisher", new {id = publisher.Id});
            }
            catch (OperationCanceledException e)
            {
                return View("Error");
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }
    }
}
