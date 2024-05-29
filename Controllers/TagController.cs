using backend_lembrol.Service;
using backend_lembrol.Dto;
using backend_lembrol.Entity;
using Microsoft.AspNetCore.Mvc;
using backend_lembrol.Exceptions;

namespace backend_lembrol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly TagService _tagService;
        
        public TagController(TagService tagService)
        {
            _tagService = tagService;
        }

        [HttpPost("create_tag")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostTag([FromBody] TagDto tag)
        {
            try
            {
                await _tagService.CreateTag(tag);
                return Created();
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("edit_tag/{id}")]
        [ProducesResponseType(typeof(CompleteTagDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutTag([FromRoute] string id,[FromBody] CompleteUpdateTagDto tag)
        {
            try
            {
                await _tagService.UpdateTag(id, tag);
                var updatedtag = await _tagService.GetTagById(id);
                return Ok(updatedtag);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("list_tags")]
        [ProducesResponseType(typeof(List<CompleteTagDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> GetTags()
        {
            try
            {
                var tags = await _tagService.GetTags();
                return Ok(tags);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("get_tag/{id}")]
        [ProducesResponseType(typeof(CompleteTagDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetTag(string id)
        {
            try
            {
                var tag = await _tagService.GetTagById(id);
                return Ok(tag);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("state_tag/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> TagState([FromRoute] string id)
        {
            try
            {
                await _tagService.TagState(id);
                return Ok("Tag Altered");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete("delete_tag/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteTag([FromRoute] string id)
        {
            try
            {
                await _tagService.DeleteTag(id);
                return Ok("Tag Deleted");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("state_day_of_week/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DayOfWeekState([FromRoute] string id, int dayOfWeek)
        {
            try
            {
                await _tagService.DayOfWeekState(id, dayOfWeek);
                return Ok("Day of week Altered");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete_specific_date/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteSpecificDate([FromRoute] string id, DateTime date)
        {
            try
            {
                await _tagService.DeleteSpecificDate(id, date);
                return Ok("Specific Date Deleted");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("tags_of_the_day")]
        public IActionResult GetTagsByDate()
        {
            var currentDate = DateTime.Now;
            var tags = _tagService.GetTagsByDate(currentDate);
            return Ok(tags);
        }
        
    }
}



