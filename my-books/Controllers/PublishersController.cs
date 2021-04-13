using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.ActionResults;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using my_books.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase {
        private PublishersService _publishersService;

        public PublishersController(PublishersService publishersService) {
            _publishersService = publishersService;
        }

        [HttpGet("get-all-publishers")]
        public IActionResult GetAllPublishers(string sortBy, string searchString) {
 
            try {
                var _result = _publishersService.GetAllPublishers(sortBy, searchString);

                return Ok(_result);

            } catch (Exception) {

                return BadRequest("Sorry, We could not load Publishers");
            }
        }

        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] PublisherVM publisher) {

            try {
                var _result = _publishersService.AddPublisher(publisher);
                return Created(nameof(AddPublisher), _result);
            } catch (PublisherNameException ex) {
                return BadRequest($"{ex.Message}, Publisher Name: {ex.PublisherName}");
            
            } catch (Exception ex) {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-publisher-by-id/{id}")]
        public IActionResult GetPublisherById(int id) {

            var _result = _publishersService.GetPublisherById(id);

            if (_result != null) {
                return Ok(_result);
            } else {

                return NotFound();
            }

        }

        [HttpGet("get-publisher-books-with-authors/{id}")]
        public IActionResult GetPublisherData(int id) {
            var _result = _publishersService.GetPublisherData(id);

            return Ok(_result);
        }

        [HttpDelete("delet-publisher-by-id/{id}")]
        public IActionResult DeletePublisherById(int id) {

            try {
                _publishersService.DeletePublisherById(id);
                return Ok();

            }catch (Exception ex) {

                return BadRequest(ex.Message);
            }
        }

    }
}
