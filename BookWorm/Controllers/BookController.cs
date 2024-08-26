using BookWorm.Data.UnitOfWork;
using BookWorm.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.Controllers
{
    public class BookController : Controller
    {
        
        public async Task<ViewResult> Details(int id)
        {
            //var book = await _unitOfWork.BookRepository.GetBookByIdAsync(id);
            var book = new Book("The Great Gatsby")
            {
                Introduction = "The Great Gatsby is a novel by American writer F. Scott Fitzgerald. Set in the Jazz Age on Long Island, the novel depicts narrator Nick Carraway's interactions with mysterious millionaire Jay Gatsby and Gatsby's obsession to reunite with his former lover, Daisy Buchanan.",
                Summary = "The novel is set in West Egg and East Egg, affluent enclaves on Long Island, New York. The story primarily concerns the young and mysterious millionaire Jay Gatsby and his quixotic passion and obsession with the beautiful former debutante Daisy Buchanan. Considered to be Fitzgerald's magnum opus, The Great Gatsby explores themes of decadence, idealism, resistance to change, social upheaval, and excess, creating a portrait of the Jazz Age or the Roaring Twenties that has been described as a cautionary tale regarding the American Dream.",
                PagesNumber = 218,
                CoverPhoto = new ApplicationImage
                {
                    FileName = "The Great Gatsby.jpg",
                },
                Publisher = new Publisher("Charles Scribner's Sons")
                {
                    PublisherLogo = new ApplicationImage()
                    {
                        FileName = "Charles Scribner's Sons.jpg"
                    }
                },
                Authors = new List<Author>
                {
                    new Author("F. Scott Fitzgerald")
                    {
                        Photo = new ApplicationImage()
                        {
                            FileName = "F. Scott Fitzgerald.jpg"
                        }
                    },
                    new Author("F. Scott Fitzgerald")
                    {
                        Photo = new ApplicationImage()
                        {
                            FileName = "F. Scott Fitzgerald.jpg"
                        }
                    }
                },
                AssociatedGenres = new List<Genre>
                {
                    new Genre("Classic Literature")
                    {
                        Photo = new ApplicationImage()
                        {
                            FileName = "Classic Literature.jpeg"
                        }
                    },
                    new Genre("Test Genre")
                    {
                        Photo = new ApplicationImage()
                    }
                },


            };
            return View(book);
        }
    }
}
