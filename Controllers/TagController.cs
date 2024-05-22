using backend_lembrol.Service;
using backend_lembrol.Dto;
using backend_lembrol.Entity;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<List<Tag>>> PostTag([FromBody] TagDto tag)
        {
            await _tagService.CreateTag(tag);
            return Ok(await _tagService.GetTags());
        }

        [HttpPut("edit_tag/{id}")]
        public async Task<ActionResult<List<Tag>>> PutTag([FromRoute] string id,[FromBody] CompleteUpdateTagDto tag)
        {
            await _tagService.UpdateTag(id,tag);
            return Ok(await _tagService.GetTags());
        }

        [HttpGet("list_tags")]
        public async Task<ActionResult<List<CompleteTagDto>>> GetTags()
        {
            return Ok(await _tagService.GetTags());
        }

        [HttpGet("get_tag/{id}")]
        public async Task<ActionResult<CompleteTagDto>> GetTag(string id)
        {
            var tag = await _tagService.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpPut("state_tag/{id}")]
        public async Task<ActionResult<Tag>> TagState([FromRoute] string id)
        {
            await _tagService.TagState(id);
            return Ok("Tag Altered");
        }

        [HttpDelete("delete_tag/{id}")]
        public async Task<ActionResult<Tag>> DeleteTag([FromRoute] string id)
        {
            await _tagService.DeleteTag(id);
            return Ok("Tag Deleted");
        }

        [HttpPut("state_day_of_week/{id}")]
        public async Task<ActionResult<Tag>> DayOfWeekState([FromRoute] string id, int dayOfWeek)
        {
            await _tagService.DayOfWeekState(id, dayOfWeek);
            return Ok("Day of week Altered");
        }

        [HttpDelete("delete_specific_date/{id}")]
        public async Task<ActionResult<Tag>> DeleteSpecificDate([FromRoute] string id, DateTime date)
        {
            await _tagService.DeleteSpecificDate(id, date);
            return Ok("Specific Date Deleted");
        }
        
    }
}



